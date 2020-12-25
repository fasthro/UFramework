/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: network manager
 */
using System;
using UFramework.Core;
using UFramework.Network;
using System.Collections.Generic;
using System.Net.Sockets;

namespace UFramework
{
    public class NetManager : BaseManager, ISocketChannelListener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="int">channelId</typeparam>
        /// <typeparam name="int">index</typeparam>
        /// <returns></returns>
        private readonly Dictionary<int, int> _channelDict = new Dictionary<int, int>();
        private readonly List<SocketChannel> _channels = new List<SocketChannel>();

        /// <summary>
        /// 创建 Channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="protocalType"></param>
        public void CreateChannel(int channelId, ProtocalType protocalType)
        {
            if (_channelDict.ContainsKey(channelId))
            {
                Logger.Error($"Socket Channel 已经存在。ChannelId:{channelId}");
                return;
            }
            var channel = new SocketChannel(channelId, protocalType, this);
            _channelDict.Add(channelId, _channels.Count);
            _channels.Add(channel);
        }

        /// <summary>
        /// 移除 Channel
        /// </summary>
        /// <param name="channelId"></param>
        public void RemoveChannel(int channelId)
        {
            if (_channelDict.ContainsKey(channelId))
            {
                var index = _channelDict[channelId];
                var channel = _channels[index];
                channel.Dispose();

                _channels.RemoveAt(index);
                _channelDict.Remove(channelId);
            }
        }

        /// <summary>
        /// 获取 Channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public SocketChannel GetChannel(int channelId)
        {
            if (_channelDict.ContainsKey(channelId))
                return _channels[_channelDict[channelId]];
            return null;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(int channelId, string ip, int port)
        {
            var channel = GetChannel(channelId);
            if (channel != null)
                channel.Connect(ip, port);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="channelId"></param>
        public void Disconnect(int channelId)
        {
            var channel = GetChannel(channelId);
            if (channel != null)
                channel.Disconnect();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="pack"></param>
        public void Send(int channelId, SocketPack pack)
        {
            var channel = GetChannel(channelId);
            if (channel != null)
                channel.Send(pack);
        }

        public void Redirect(int channelId, string ip, int port)
        {
            var channel = GetChannel(channelId);
            if (channel != null)
                channel.Redirect(ip, port);
        }

        #region BaseManager

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _channels.Count; i++)
                _channels[i].Update();
        }

        public override void OnDestroy()
        {
            for (int i = 0; i < _channels.Count; i++)
                _channels[i].Dispose();
        }

        #endregion

        #region socket

        public void OnSocketChannelConnected(int channelId)
        {
            ThreadQueue.EnqueueMain(_OnSocketConnected, channelId);
        }

        private void _OnSocketConnected(object channelId)
        {
            Messenger.Broadcast<int>(GlobalEvent.NET_CONNECTED, (int)channelId);
            LuaCall("onSocketConnected", channelId);
        }

        public void OnSocketChannelDisconnected(int channelId)
        {
            ThreadQueue.EnqueueMain(_OnSocketDisconnected, channelId);
        }

        private void _OnSocketDisconnected(object channelId)
        {
            Messenger.Broadcast<int>(GlobalEvent.NET_DISCONNECTED, (int)channelId);
            LuaCall("onSocketDisconnected", channelId);
        }

        public void OnSocketChannelReceive(int channelId)
        {
            ThreadQueue.EnqueueMain(_OnSocketReceive, channelId);
        }

        private void _OnSocketReceive(object channelId)
        {
            var cid = (int)channelId;
            var channel = GetChannel(cid);
            if (channel != null)
            {
                if (channel.packQueue.IsEmpty())
                {
                    channel.packQueue.Swap();
                }
                while (!channel.packQueue.IsEmpty())
                {
                    var pack = channel.packQueue.Dequeue();
                    if (pack.layer == ProcessLayer.All)
                    {
                        Messenger.Broadcast<int, SocketPack>(GlobalEvent.NET_RECEIVED, cid, pack);
                        LuaCall("onSocketReceive", cid, pack);
                    }
                    else if (pack.layer == ProcessLayer.Lua)
                    {
                        LuaCall("onSocketReceive", cid, pack);
                    }
                    else if (pack.layer == ProcessLayer.CSharp)
                    {
                        Logger.Debug($"c# socket receive channelId: {cid} cmd: {pack.cmd}");
                        Messenger.Broadcast<int, SocketPack>(GlobalEvent.NET_RECEIVED, cid, pack);
                    }
                    pack.Recycle();
                }
            }
        }

        public void OnSocketChannelException(int channelId, SocketError error)
        {
            ThreadQueue.EnqueueMain(_OnSocketException, channelId, error);
        }

        private void _OnSocketException(object channelId, object error)
        {
            Messenger.Broadcast<int, SocketError>(GlobalEvent.NET_EXCEPTION, (int)channelId, (SocketError)error);
            LuaCall("onSocketException", channelId, error);
        }

        #endregion
    }
}
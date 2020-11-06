/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: TCPManager(Socket Manager)
 */
using UFramework.Pool;
using UFramework.Network;
using System.Collections.Generic;
using Google.Protobuf;
using LuaInterface;
using UnityEngine;

namespace UFramework
{
    public class TCPManager : BaseManager
    {
        /// <summary>
        /// Socket Client
        /// </summary>
        private SocketClient _client;

        /// <summary>
        /// 消息处理队列
        /// </summary>
        /// <typeparam name="SocketEventArgs"></typeparam>
        /// <returns></returns>
        private Queue<SocketEventArgs> _socketEventArgQueue = new Queue<SocketEventArgs>();

        /// <summary>
        /// 会话记录保证发送与接收一一对应
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        private Dictionary<int, int> _sessionDictionary = new Dictionary<int, int>();

        /// <summary>
        /// connected
        /// </summary>
        /// <value></value>
        public bool isConnected
        {
            get { return _client != null && _client.isConnected; }
        }

        protected override void OnInitialize()
        {
            // 初始化对象池
            ObjectPool<SocketEventArgs>.Instance.Initialize(10, 10);

            _client = new SocketClient(OnSocketEventCallback);
            _client.connectTimeout = 5000;
            _client.receiveBufferSize = 4096;
            _client.sendBufferSize = 4096;
        }

        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="args"></param>
        private void OnSocketEventCallback(SocketEventArgs args)
        {
            _socketEventArgQueue.Enqueue(args);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (_client != null) _client.Update();

            if (_socketEventArgQueue.Count > 0)
            {
                var args = _socketEventArgQueue.Dequeue();

                switch (args.socketState)
                {
                    case SocketState.Received:
                        var pack = args.socketPack;
                        if (_sessionDictionary.ContainsKey(pack.cmd))
                        {
                            pack.valid = pack.sessionId == _sessionDictionary[pack.cmd];
                        }
                        else
                        {
                            pack.valid = true;
                        }
                        break;
                    case SocketState.Connected:
                        break;
                    case SocketState.Disconnected:
                        if (string.IsNullOrEmpty(args.error))
                            Debug.LogError("socket exception: [" + args.exception.ToString() + "]");
                        else
                            Debug.LogError("socket exception: [" + args.exception.ToString() + "] error: " + args.error);
                        break;
                    default:
                        break;
                }
                args.Recycle();
            }
        }

        public void Connecte(string ip, int port)
        {
            if (!isConnected)
            {
                _client.Connect(ip, port);
            }
        }

        public void Send(SocketPack pack)
        {
            if (isConnected)
            {
                var sessionId = _client.Send(pack);
                if (!_sessionDictionary.ContainsKey(pack.cmd))
                {
                    _sessionDictionary.Add(pack.cmd, sessionId);
                }
                else
                {
                    _sessionDictionary[pack.cmd] = sessionId;
                }
            }
        }

        public void Send(int cmd, IMessage message)
        {
            if (isConnected)
            {
                var sessionId = _client.Send(cmd, message);
                if (!_sessionDictionary.ContainsKey(cmd))
                {
                    _sessionDictionary.Add(cmd, sessionId);
                }
                else
                {
                    _sessionDictionary[cmd] = sessionId;
                }
            }
        }

        public void Send(int cmd, LuaByteBuffer luabyte)
        {
            Send(SocketPackFactory.CreateWriter(cmd, luabyte));
        }

        protected override void OnLateUpdate()
        {

        }

        protected override void OnFixedUpdate()
        {

        }

        protected override void OnDispose()
        {
            _client?.Disconnecte();
        }
    }
}
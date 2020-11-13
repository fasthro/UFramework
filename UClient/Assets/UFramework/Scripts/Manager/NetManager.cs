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
using UFramework.Tools;
using System;

namespace UFramework
{
    public class NetManager : BaseManager, ISocketListener
    {
        /// <summary>
        /// Socket Client
        /// </summary>
        private SocketClient _client;

        /// <summary>
        /// 消息处理队列
        /// </summary>
        /// <typeparam name="SocketPack"></typeparam>
        /// <returns></returns>
        private DoubleQueue<SocketPack> _packs = new DoubleQueue<SocketPack>();

        /// <summary>
        /// connected
        /// </summary>、
        /// <value></value>
        public bool isConnected
        {
            get { return _client != null && _client.isConnected; }
        }

        protected override void OnInitialize()
        {
            _client = new SocketClient(this);
            _client.connectTimeout = 5000;
            _client.receiveBufferSize = 4096;
            _client.sendBufferSize = 4096;
        }

        public void Connecte(string ip, int port)
        {
            if (!isConnected)
            {
                _client.Connect(ip, port);
            }
        }

        #region ISocketListener
        public void OnConnected()
        {
            Log("net connected!");
            var pack = new SocketPackLine();
            pack.WriteInt(100);
            pack.Pack();
            _client.Send(pack);
        }

        public void OnDisconnected()
        {
            Log("net disconnected!");
        }

        public void OnReceive(SocketPack pack)
        {
            _packs.Enqueue(pack);
        }

        public void OnNetworkError(SocketError code, Exception error)
        {
            LogError(string.Format("net error: {0}. {1}", code.ToString(), error != null ? error.ToString() : ""));
        }

        #endregion

        #region BaseManager
        protected override void OnUpdate(float deltaTime)
        {
            _client.OnUpdate();
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

        #endregion

        protected override void Log(object message)
        {
            base.Log(string.Format("[NetManager] {0}", message.ToString()));
        }

        protected override void LogError(object message)
        {
            base.LogError(string.Format("[NetManager] {0}", message.ToString()));
        }
    }
}
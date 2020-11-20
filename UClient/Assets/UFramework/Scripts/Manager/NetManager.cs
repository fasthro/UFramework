/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: TCPManager(Socket Manager)
 */
using UFramework.Network;
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
        private DoubleQueue<SocketPack> _packQueue = new DoubleQueue<SocketPack>();

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
            _client.connectTimeout = 15000;
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

        public void Sendbyte(byte[] value)
        {
            _client.Send(value);
        }

        #region ISocketListener
        public void OnConnected() { ThreadQueue.EnqueueMain(_OnConnected); }
        private void _OnConnected()
        {
            LuaCall("onConnected");
        }

        public void OnDisconnected() { ThreadQueue.EnqueueMain(_OnDisconnected); }
        private void _OnDisconnected()
        {
            LuaCall("onDisconnected");
        }

        public void OnReceive(SocketPack pack)
        {
            _packQueue.Enqueue(pack);
            ThreadQueue.EnqueueMain(_OnReceive);
        }

        private void _OnReceive()
        {
            _packQueue.Swap();
            while (!_packQueue.IsEmpty())
            {
                var pack = _packQueue.Dequeue();
                Log("onreceive len = " + pack.rawDataSize);
                LuaCall("onReceive", pack);
            }
        }

        public void OnNetworkError(SocketError code, Exception error) { ThreadQueue.EnqueueMain(_OnNetworkError, code, error); }
        private void _OnNetworkError(object code, object error)
        {
            LuaCall("onNetworkError");
        }

        #endregion

        #region BaseManager
        protected override void OnUpdate(float deltaTime)
        {
            _client?.OnUpdate();
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
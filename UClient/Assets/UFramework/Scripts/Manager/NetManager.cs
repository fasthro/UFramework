/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: TCPManager(Socket Manager)
 */
using UFramework.Network;
using UFramework.Tools;
using System;
using System.Text;

namespace UFramework
{
    public class NetManager : BaseManager, ISocketListener
    {
        /// <summary>
        /// connected
        /// </summary>、
        /// <value></value>
        public bool isConnected { get { return _client != null && _client.isConnected; } }

        /// <summary>
        /// 重定向中
        /// </summary>
        /// <value></value>
        public bool isRedirecting { get; private set; }

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

        protected override void OnInitialize()
        {
            isRedirecting = false;
            _packQueue.Clear();
            CreateSocketClient();
        }

        /// <summary>
        /// 创建 socekt client
        /// </summary>
        private void CreateSocketClient()
        {
            _client = new SocketClient(this);
            _client.connectTimeout = 15000;
            _client.receiveBufferSize = 8192;
            _client.sendBufferSize = 8192;
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connecte(string ip, int port)
        {
            if (!isConnected)
                _client.Connect(ip, port);
        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Redirect(string ip, int port)
        {
            isRedirecting = true;

            if (_client != null)
            {
                _client.Disconnecte();
                _client = null;
            }

            _packQueue.Clear();
            CreateSocketClient();
            Connecte(ip, port);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnecte()
        {
            if (isConnected)
                _client.Disconnecte();
        }

        /// <summary>
        /// send pack
        /// </summary>
        /// <param name="value"></param>
        public void Send(SocketPack pack)
        {
            _client.Send(pack);
        }

        #region ISocketListener

        public void OnSocketConnected()
        {
            ThreadQueue.EnqueueMain(_OnSocketConnected);
        }

        private void _OnSocketConnected()
        {
            isRedirecting = false;
            LuaCall("onSocketConnected");
        }

        public void OnSocketDisconnected()
        {
            _packQueue.Clear();
            ThreadQueue.EnqueueMain(_OnSocketDisconnected);
        }

        private void _OnSocketDisconnected()
        {
            if (isRedirecting) return;
            LuaCall("onSocketDisconnected");
        }

        public void OnSocketReceive(SocketPack pack)
        {
            _packQueue.Enqueue(pack);
            ThreadQueue.EnqueueMain(_OnSocketReceive);
        }

        private void _OnSocketReceive()
        {
            _packQueue.Swap();
            while (!_packQueue.IsEmpty())
            {
                LuaCall("onSocketReceive", _packQueue.Dequeue());
            }
        }

        public void OnSocketException(Exception exception)
        {
            _packQueue.Clear();
            ThreadQueue.EnqueueMain(_OnSocketException, exception);
        }

        private void _OnSocketException(object exception)
        {
            LuaCall("onSocketException", exception == null ? "" : exception.ToString());
        }

        #endregion

        #region BaseManager
        protected override void OnUpdate(float deltaTime)
        {
            _client?.OnUpdate();
        }

        protected override void OnLateUpdate()
        { }

        protected override void OnFixedUpdate()
        { }

        protected override void OnDispose()
        {
            _client?.Disconnecte();
        }

        protected override void Log(object message)
        {
            base.Log(string.Format("[NetManager] {0}", message.ToString()));
        }

        protected override void LogError(object message)
        {
            base.LogError(string.Format("[NetManager] {0}", message.ToString()));
        }

        #endregion
    }
}
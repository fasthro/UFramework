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
        public bool isConnected { get { return _client != null && _client.isConnected; } }

        protected override void OnInitialize()
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
        /// 设置网络包解析选项
        /// </summary>
        /// <param name="option"></param>
        public void SetPackOption(SocketPackOption option)
        {
            _client.packOption = option;
        }

        /// <summary>
        /// send string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        public void Send(string value, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            Send(encoding.GetBytes(value));
        }

        /// <summary>
        /// send bytes
        /// </summary>
        /// <param name="value"></param>
        public void Send(byte[] value)
        {
            _client.Send(value);
        }

        #region ISocketListener

        public void OnSocketConnected()
        {
            ThreadQueue.EnqueueMain(_OnSocketConnected);
        }

        private void _OnSocketConnected()
        {
            LuaCall("onSocketConnected");
        }

        public void OnSocketDisconnected()
        {
            _packQueue.Clear();
            ThreadQueue.EnqueueMain(_OnSocketDisconnected);
        }

        private void _OnSocketDisconnected()
        {
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
                var pack = _packQueue.Dequeue();
                switch (_client.packOption)
                {
                    case SocketPackOption.Linear:
                        LuaCall("onSocketReceive", pack.ToPack<SocketPackLinear>());
                        break;
                    case SocketPackOption.Protobuf:
                        LuaCall("onSocketReceive", pack.ToPack<SocketPackProtobuf>());
                        break;
                    case SocketPackOption.Sproto:
                        LuaCall("onSocketReceive", pack.ToPack<SocketPackSproto>());
                        break;
                    case SocketPackOption.RawByte:
                        LuaCall("onSocketReceive", pack.ToPack<SocketPackRawByte>());
                        break;
                }
            }
        }

        public void OnSocketException(Exception exception)
        {
            _packQueue.Clear();
            ThreadQueue.EnqueueMain(_OnSocketException, exception);
        }

        private void _OnSocketException(object exception)
        {
            LuaCall("onSocketException", exception.ToString());
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
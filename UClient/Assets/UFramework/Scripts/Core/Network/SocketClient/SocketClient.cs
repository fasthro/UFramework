/*
 * @Author: fasthro
 * @Date: 2019-08-08 11:33:05
 * @Description: socket client
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Google.Protobuf;
using System.Collections.Generic;
using UFramework.Messenger;
using UFramework.Pool;
using UFramework.Tools;

namespace UFramework.Network
{
    /// <summary>
    /// socket state
    /// </summary>
    public enum SocketState
    {
        Connected,
        Disconnected,
        Send,
        Received,
    }

    public enum SocketException
    {
        Timeout,
        Connect,
        Receive,
        Send,
    }

    /// <summary>
    /// socket event callback args
    /// </summary>
    public class SocketEventArgs : IPoolObject
    {
        // socket state
        public SocketState socketState { get; private set; }

        // socket pack
        public SocketPack socketPack { get; private set; }

        // cmd
        public int cmd { get; private set; }

        // exception
        public SocketException exception { get; private set; }

        // error
        public string error { get; private set; }

        public SocketEventArgs()
        {
        }

        #region pool

        public bool isRecycled { get; set; }

        public static SocketEventArgs Allocate(SocketState socketState)
        {
            var obj = ObjectPool<SocketEventArgs>.Instance.Allocate();
            obj.socketState = socketState;
            return obj;
        }

        public static SocketEventArgs Allocate(SocketState socketState, SocketPack socketPack)
        {
            var obj = ObjectPool<SocketEventArgs>.Instance.Allocate();
            obj.socketState = socketState;
            obj.socketPack = socketPack;
            return obj;
        }

        public static SocketEventArgs Allocate(SocketState socketState, int cmd)
        {
            var obj = ObjectPool<SocketEventArgs>.Instance.Allocate();
            obj.socketState = socketState;
            obj.cmd = cmd;
            return obj;
        }

        public static SocketEventArgs Allocate(SocketState socketState, SocketException exception, string error)
        {
            var obj = ObjectPool<SocketEventArgs>.Instance.Allocate();
            obj.socketState = socketState;
            obj.exception = exception;
            obj.error = error;
            return obj;
        }

        public void Recycle()
        {
            ObjectPool<SocketEventArgs>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }
        #endregion
    }

    public class SocketClient
    {
        /// <summary>
        /// socket async object
        /// </summary>
        class AsyncObject
        {
            public Socket socket { get; set; }
            public List<int> cmds = new List<int>();
        }

        // socket
        private Socket _client;

        // 数据接收线程
        private Thread _recThread;

        // 事件监听
        private event UCallback<SocketEventArgs> _listener;

        #region receive

        // 数据接收池
        private byte[] _recCache;
        // 数据接收处理器
        private SocketReceiver _receiver;
        // 接收的数据包
        private SocketPack _recPack;

        #endregion

        #region send

        // 发送包头
        public SocketPackHeader _sendPackHeader;
        // 发送队列
        private Queue<byte[]> _sendQueue;
        // 发送命令队列
        private Queue<int> _sendCmdQueue;
        // 发送缓存池
        private ByteCache _sendCache;
        // 发送命令数组
        private AsyncObject _sendAsyncObj;
        // 发送数据大小
        private int _sendSize;

        #endregion

        // 双队列事件机制
        private DoubleQueue<SocketEventArgs> _eventQueue;

        public string ip { get; private set; }
        public int port { get; private set; }
        public bool isConnected { get; private set; }
        public bool isConnecting { get; private set; }

        private float _connectDecreaseTime;
        private int _connectTimeout = 5000;
        private int _receiveBufferSize = 4096;
        private int _sendBufferSize = 4096;

        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int connectTimeout
        {
            get { return _connectTimeout; }
            set { _connectTimeout = value; }
        }

        /// <summary>
        /// 接收数据池大小
        /// </summary>
        public int receiveBufferSize
        {
            get { return _receiveBufferSize; }
            set { _receiveBufferSize = value; }
        }

        /// <summary>
        /// 最大发送数据大小
        /// </summary>
        public int sendBufferSize
        {
            get { return _sendBufferSize; }
            set { _sendBufferSize = value; }
        }

        /// <summary>
        /// socket client
        /// </summary>
        /// <param name="callback"></param>
        public SocketClient(UCallback<SocketEventArgs> callback)
        {
            _listener = callback;
            _recCache = new byte[receiveBufferSize];
            _receiver = new SocketReceiver();
            _sendPackHeader = new SocketPackHeader();
            _eventQueue = new DoubleQueue<SocketEventArgs>(128);
            _sendQueue = new Queue<byte[]>();
            _sendCmdQueue = new Queue<int>();
            _sendCache = new ByteCache();
            _sendAsyncObj = new AsyncObject();
        }

        public void Update()
        {
            if (isConnecting)
            {
                _connectDecreaseTime -= Time.unscaledDeltaTime;
                if (_connectDecreaseTime <= 0)
                {
                    Exception(SocketException.Timeout);
                }
            }

            if (isConnected)
            {
                ProcessSend();
            }

            ProcessEvent();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port)
        {
            this.port = port;

            IPAddress address = null;
            if (IPAddress.TryParse(ip, out address))
                this.ip = ip;
            else
                this.ip = GetIP(ip);

            Connect();
        }

        private void Connect()
        {
            if (isConnecting) return;

            try
            {
                string newIp = "";
                string connetIp = ip;
                // ipv6 & ipv4
                AddressFamily newAddressFamily = AddressFamily.InterNetwork;
                IPv6SupportMidleware.getIPType(ip, port.ToString(), out newIp, out newAddressFamily);
                if (!string.IsNullOrEmpty(newIp)) { connetIp = newIp; }

                Debug.Log("socket connect to server. " + ip + ":" + port + (string.IsNullOrEmpty(newIp) ? " ipv4" : " ipv6"));

                // 解析IP地址
                IPAddress ipAddress = IPAddress.Parse(connetIp);
                IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

                // 创建 Socket
                _client = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _client.NoDelay = true;

                _sendAsyncObj.socket = _client;
                _client.BeginConnect(ipEndpoint, new AsyncCallback(OnConnetSucceed), _client);
                isConnecting = true;
                _connectDecreaseTime = (float)connectTimeout / 1000f;
            }
            catch (System.Exception e)
            {
                Exception(SocketException.Connect, e);
            }
        }

        /// <summary>
        /// 重新连接服务器
        /// </summary>
        public void ReConnect()
        {
            OnDisconnected();
            Connect();
        }

        private void OnConnetSucceed(IAsyncResult iar)
        {
            try
            {
                Debug.Log("socket connect to server succeed.");
                ((Socket)iar.AsyncState).EndConnect(iar);
                isConnected = true;
                isConnecting = false;

                AddEventQueue(SocketEventArgs.Allocate(SocketState.Connected));

                _recThread = new Thread(new ThreadStart(OnReceive));
                _recThread.IsBackground = true;
                _recThread.Start();
            }
            catch (Exception e)
            {
                Exception(SocketException.Connect, e);
            }
        }

        public int Send(SocketPack pack)
        {
            if (!isConnected || _client == null || !_client.Connected) return -1;
            _sendCmdQueue.Enqueue(pack.cmd);
            _sendQueue.Enqueue(_sendPackHeader.Write(pack.cmd, pack.data));
            return _sendPackHeader.sessionId;
        }

        public int Send(int cmd, IMessage message)
        {
            return Send(new SocketPack(cmd, message));
        }

        private void OnSend(IAsyncResult iar)
        {
            try
            {
                var obj = ((AsyncObject)iar.AsyncState);
                obj.socket.EndSend(iar);
                for (int i = 0; i < obj.cmds.Count; i++)
                {
                    AddEventQueue(SocketEventArgs.Allocate(SocketState.Send, obj.cmds[i]));
                }
            }
            catch (Exception e)
            {
                Exception(SocketException.Send, e);
            }
        }

        private void ProcessSend()
        {
            try
            {
                _sendCache.Clear();
                if (_sendQueue.Count > 0)
                {
                    _sendCache.Clear();
                }
                _sendAsyncObj.cmds.Clear();
                _sendSize = 0;
                for (; _sendQueue.Count > 0;)
                {
                    byte[] data = _sendQueue.Peek();
                    if ((_sendSize > 0 && data.Length + _sendSize > sendBufferSize) || data == null) break;
                    _sendSize += data.Length;
                    _sendCache.Write(data, data.Length);
                    _sendQueue.Dequeue();
                    _sendAsyncObj.cmds.Add(_sendCmdQueue.Dequeue());
                }
                if (_sendCache.size > 0)
                {
                    bool succeed = false;
                    byte[] data = _sendCache.Read(_sendCache.size, ref succeed);
                    if (succeed)
                    {
                        _client.BeginSend(data, 0, data.Length, SocketFlags.DontRoute, new AsyncCallback(OnSend), _sendAsyncObj);
                    }
                }
            }
            catch (Exception e)
            {
                Exception(SocketException.Send, e);
            }
        }

        private void OnReceive()
        {
            while (true)
            {
                try
                {
                    if (_client.Connected)
                    {
                        // 接受数据
                        int bSize = _client.Receive(_recCache);
                        if (bSize > 0)
                        {
                            // 向接受者Push数据
                            _receiver.Push(_recCache, bSize);
                            // 尝试在向接受者获取Pack
                            while ((_recPack = _receiver.TryGetPack()) != null)
                            {
                                AddEventQueue(SocketEventArgs.Allocate(SocketState.Received, _recPack));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Exception(SocketException.Receive, e);
                    break;
                }
            }
        }

        private void Exception(SocketException exception, Exception e = null)
        {
            OnDisconnected();
            if (e != null)
                AddEventQueue(SocketEventArgs.Allocate(SocketState.Disconnected, exception, e.ToString()));
            else
                AddEventQueue(SocketEventArgs.Allocate(SocketState.Disconnected, exception, null));
        }

        public void Disconnecte()
        {
            if (!isConnected) return;
            OnDisconnected();
            AddEventQueue(SocketEventArgs.Allocate(SocketState.Disconnected));
        }

        private void OnDisconnected()
        {
            Debug.Log("socket disconnected && closed.");

            isConnected = false;
            isConnecting = false;

            if (_recThread != null)
                _recThread.Abort();
            _recThread = null;

            if (_client != null && _client.Connected)
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
            }
            _client = null;
        }

        private void AddEventQueue(SocketEventArgs arg)
        {
            _eventQueue.Enqueue(arg);
        }

        private void ProcessEvent()
        {
            _eventQueue.Swap();

            while (!_eventQueue.IsEmpty())
            {
                _listener.InvokeGracefully(_eventQueue.Dequeue());
            }
        }

        public static string GetIP(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
            return ipEndPoint.Address.ToString();
        }
    }
}
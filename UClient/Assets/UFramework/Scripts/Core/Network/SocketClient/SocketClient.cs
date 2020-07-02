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
        ProcessReceive,
        Send,
        ProcessSend,
        Exception,
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
        #region config
        /// <summary>
        /// 连接超时时间
        /// </summary>
        readonly static int ConnectedTimeout = 15000;

        /// <summary>
        /// 接收数据池大小
        /// </summary>
        readonly static int ReceiveCacheSize = 4096;

        /// <summary>
        /// 最大发送数据大小
        /// </summary>
        readonly static int SendMaxSize = 15360;

        /// <summary>
        /// 发送间隔时间
        /// </summary>
        readonly static float SendIntervalTime = 0.2f;

        #endregion

        /// <summary>
        /// socket async object
        /// </summary>
        class AsyncObject
        {
            public Socket socket { get; set; }
            public List<int> cmds = new List<int>();
        }

        // socket
        private Socket m_clientSocket;

        // 数据接收线程
        private Thread m_recThread;

        // 事件回调
        private event UCallback<SocketEventArgs> m_eventCallback;

        #region connect
        // 连接成功之后主线程通知
        private bool m_connectedNotification = false;
        #endregion

        #region receive
        // 数据接收池
        private byte[] m_recCache;
        // 数据接收处理器
        private SocketReceiver m_receiver;
        // 接收的数据包
        private SocketPack m_recPack;
        // 双队列接收处理机制
        private DoubleQueue<SocketPack> m_recDQueue;
        // 主线程处理接受异常
        private bool m_mainThreadRecException;
        // 异常
        private Exception m_exception;
        #endregion

        #region send
        // 发送包头
        public SocketPackHeader m_sendPackHeader;
        // 发送队列
        private Queue<byte[]> m_sendQueue;
        // 发送命令队列
        private Queue<int> m_sendCmdQueue;
        // 发送缓存池
        private ByteCache m_sendCache;
        // 发送命令数组
        private AsyncObject m_sendAsyncObj;
        // 发送数据大小
        private int m_sendSize;
        // 发送开始时间
        private float m_sendStartTime;
        #endregion

        public string ip { get; private set; }
        public int port { get; private set; }
        public bool isConnected { get; private set; }
        public bool isSocketConnected { get { return m_clientSocket != null && m_clientSocket.Connected; } }

        /// <summary>
        /// socket client
        /// </summary>
        /// <param name="callback"></param>
        public SocketClient(UCallback<SocketEventArgs> callback)
        {
            m_eventCallback = callback;
            m_recCache = new byte[ReceiveCacheSize];
            m_receiver = new SocketReceiver();
            m_sendPackHeader = new SocketPackHeader();
            m_recDQueue = new DoubleQueue<SocketPack>(128);
            m_sendQueue = new Queue<byte[]>();
            m_sendCmdQueue = new Queue<int>();
            m_sendCache = new ByteCache();
            m_sendAsyncObj = new AsyncObject();
        }

        public void Update()
        {
            if (!isConnected) return;
            ProcessConnecte();
            ProcessReceive();
            ProcessSend();
            ProcessMainThreadRecException();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port)
        {
            IPAddress address = null;
            if (IPAddress.TryParse(ip, out address))
            {
                this.ip = ip;
            }
            else
            {
                this.ip = GetIP(ip);
            }
            this.port = port;
            Connect();
        }

        private void Connect()
        {
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
                m_clientSocket = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);
                m_clientSocket.NoDelay = true;

                m_sendAsyncObj.socket = m_clientSocket;

                // 异步连接
                IAsyncResult result = m_clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(OnConnetSucceed), m_clientSocket);

                // 连接超时
                bool success = result.AsyncWaitHandle.WaitOne(ConnectedTimeout, true);
                if (!success)
                {
                    Exception(SocketException.Timeout, null);
                }
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
                m_connectedNotification = true;
                m_recThread = new Thread(new ThreadStart(OnReceive));
                m_recThread.IsBackground = true;
                m_recThread.Start();
            }
            catch (Exception e)
            {
                Exception(SocketException.Connect, e);
            }
        }

        public void ProcessConnecte()
        {
            if (!isConnected || !m_connectedNotification) return;
            m_connectedNotification = false;
            BroadcastConnected();
        }

        public int Send(SocketPack pack)
        {
            if (!isConnected || m_clientSocket == null || !m_clientSocket.Connected) return -1;
            m_sendCmdQueue.Enqueue(pack.cmd);
            m_sendQueue.Enqueue(m_sendPackHeader.Write(pack.cmd, pack.data));
            return m_sendPackHeader.sessionId;
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
                    var cmd = obj.cmds[i];
                    BroadcastSend(cmd);
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
                if (Time.time - m_sendStartTime < SendIntervalTime) return;

                m_sendCache.Clear();
                m_sendAsyncObj.cmds.Clear();
                m_sendSize = 0;
                for (; m_sendQueue.Count > 0;)
                {
                    byte[] data = m_sendQueue.Peek();
                    if ((m_sendSize > 0 && data.Length + m_sendSize > SendMaxSize) || data == null) break;
                    m_sendSize += data.Length;
                    m_sendCache.Write(data, data.Length);
                    m_sendQueue.Dequeue();
                    m_sendAsyncObj.cmds.Add(m_sendCmdQueue.Dequeue());
                }
                if (m_sendCache.size > 0)
                {
                    bool succeed = false;
                    byte[] data = m_sendCache.Read(m_sendCache.size, ref succeed);
                    if (succeed)
                    {
                        m_sendStartTime = Time.time;
                        m_clientSocket.BeginSend(data, 0, data.Length, SocketFlags.DontRoute, new AsyncCallback(OnSend), m_sendAsyncObj);
                    }
                }
            }
            catch (Exception e)
            {
                Exception(SocketException.ProcessSend, e);
            }
        }

        private void OnReceive()
        {
            while (true)
            {
                if (!m_clientSocket.Connected)
                {
                    isConnected = false;
                    break;
                }
                try
                {
                    // 接受数据
                    int bSize = m_clientSocket.Receive(m_recCache);
                    if (bSize > 0)
                    {
                        // 向接受者Push数据
                        m_receiver.Push(m_recCache, bSize);
                        // 尝试在向接受者获取Pack
                        while ((m_recPack = m_receiver.TryGetPack()) != null)
                        {
                            m_recDQueue.Enqueue(m_recPack);
                        }
                    }
                }
                catch (Exception e)
                {
                    if (isConnected)
                    {
                        m_mainThreadRecException = true;
                        m_exception = e;
                    }
                    break;
                }
            }
        }

        private void ProcessReceive()
        {
            m_recDQueue.Swap();

            while (!m_recDQueue.IsEmpty())
            {
                var pack = m_recDQueue.Dequeue();
                BroadcastReceived(pack);
            }
        }

        private void ProcessMainThreadRecException()
        {
            if (m_mainThreadRecException)
            {
                m_mainThreadRecException = false;
                Exception(SocketException.Receive, m_exception);
            }
        }

        private void Exception(SocketException exception, Exception e = null)
        {
            Debug.LogError("socket exception: [" + exception.ToString() + "] error: " + e.ToString());
            OnDisconnected();
            BroadcastException(exception, e.ToString());
        }

        public void Disconnecte()
        {
            if (!isConnected) return;
            OnDisconnected();
            BroadcastDisconnected();
        }

        private void OnDisconnected()
        {
            Debug.Log("socket disconnected && closed.");

            isConnected = false;

            if (m_recThread != null)
                m_recThread.Abort();
            m_recThread = null;

            if (m_clientSocket != null && m_clientSocket.Connected)
            {
                m_clientSocket.Shutdown(SocketShutdown.Both);
                m_clientSocket.Close();
            }
            m_clientSocket = null;
        }

        #region broadcast
        private void BroadcastConnected()
        {
            m_eventCallback.InvokeGracefully(SocketEventArgs.Allocate(SocketState.Connected));
        }

        private void BroadcastSend(int cmd)
        {
            m_eventCallback.InvokeGracefully(SocketEventArgs.Allocate(SocketState.Send, cmd));
        }

        private void BroadcastReceived(SocketPack pack)
        {
            m_eventCallback.InvokeGracefully(SocketEventArgs.Allocate(SocketState.Received, pack));
        }

        private void BroadcastDisconnected()
        {
            m_eventCallback.InvokeGracefully(SocketEventArgs.Allocate(SocketState.Disconnected));
        }

        private void BroadcastException(SocketException exception, string error)
        {
            m_eventCallback.InvokeGracefully(SocketEventArgs.Allocate(SocketState.Disconnected, exception, error));
        }
        #endregion

        public static string GetIP(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
            return ipEndPoint.Address.ToString();
        }
    }
}
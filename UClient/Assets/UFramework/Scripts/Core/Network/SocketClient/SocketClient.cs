/*
 * @Author: fasthro
 * @Date: 2019-08-08 11:33:05
 * @Description: socket client
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UFramework.Network
{
    public enum SocketStatus
    {
        Connected,
        Disconnected,
        Send,
        Received,
    }

    public enum SocketError
    {
        Timeout,
        Connect,
        Receive,
        Send,
    }

    public class SocketClient
    {
        readonly static int HEAD_SIZE = 16;

        /// <summary>
        /// ip
        /// </summary>
        /// <value></value>
        public string ip { get; private set; }

        /// <summary>
        /// port
        /// </summary>
        /// <value></value>
        public int port { get; private set; }

        /// <summary>
        /// 已连接
        /// </summary>
        /// <value></value>
        public bool isConnected { get; private set; }

        /// <summary>
        /// 连接中
        /// </summary>
        /// <value></value>
        public bool isConnecting { get; private set; }

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

        private Socket _client;
        private Thread _thread;
        private ISocketListener _listener;

        private byte[] _buffer;
        private AutoByteArray _sender;
        private AutoByteArray _receiver;
        private FixedByteArray _header;

        private int _packSize;
        private int _cmd;
        private long _session;
        private short _packType;

        private float _connectDecreaseTime;
        private int _connectTimeout = 5000;
        private int _receiveBufferSize = 4096;
        private int _sendBufferSize = 4096;

        private bool _isSending;
        private bool _isBodyParsing;
        private bool _isException;

        /// <summary>
        /// socket client
        /// </summary>
        /// <param name="callback"></param>
        public SocketClient(ISocketListener listener)
        {
            _buffer = new byte[receiveBufferSize];
            _listener = listener;
            _sender = new AutoByteArray();
            _receiver = new AutoByteArray();
            _header = new FixedByteArray(HEAD_SIZE);
        }

        public void OnUpdate()
        {
            if (isConnecting)
            {
                _connectDecreaseTime -= Time.unscaledDeltaTime;
                if (_connectDecreaseTime <= 0)
                    OnException(SocketError.Timeout);
            }
            else if (isConnected)
            {
                ProcessSend();
            }
        }

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
                _isException = false;
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

                _client.BeginConnect(ipEndpoint, new AsyncCallback(OnConnetSucceed), _client);
                isConnecting = true;
                _connectDecreaseTime = (float)connectTimeout / 1000f;
            }
            catch (System.Exception e)
            {
                OnException(SocketError.Connect, e);
            }
        }

        private void OnConnetSucceed(IAsyncResult iar)
        {
            if (_isException) return;

            try
            {
                Debug.Log("socket connect to server succeed.");
                ((Socket)iar.AsyncState).EndConnect(iar);
                isConnected = true;
                isConnecting = false;

                _listener.OnConnected();

                _thread = new Thread(new ThreadStart(OnReceive));
                _thread.IsBackground = true;
                _thread.Start();
            }
            catch (Exception e)
            {
                OnException(SocketError.Connect, e);
            }
        }

        public void Send(SocketPack pack)
        {
            Send(pack.headData);
            Send(pack.rawData);
        }

        public void Send(byte[] data)
        {
            if (!isConnected || _client == null || !_client.Connected)
            {
                Debug.LogError("SocketClient Socket 未连接，无法发送消息!");
                return;
            }
            _sender.Write(data);
        }

        private void ProcessSend()
        {
            if (_isSending) return;
            try
            {
                if (!_sender.isEmpty())
                {
                    _isSending = true;
                    if (_sender.dataSize > sendBufferSize)
                    {
                        _client.BeginSend(_sender.Read(sendBufferSize), 0, sendBufferSize, SocketFlags.DontRoute, OnSend, null);
                    }
                    else
                    {
                        var len = _sender.dataSize;
                        _client.BeginSend(_sender.ReadAll(), 0, len, SocketFlags.DontRoute, OnSend, null);
                    }
                }
            }
            catch (Exception e)
            {
                OnException(SocketError.Send, e);
            }
        }

        private void OnSend(IAsyncResult iar)
        {
            try
            {
                _isSending = false;
            }
            catch (Exception e)
            {
                OnException(SocketError.Send, e);
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
                        int bSize = _client.Receive(_buffer);
                        if (bSize > 0)
                        {
                            _receiver.Write(_buffer, bSize);

                            var pack = new SocketPackStream(_receiver.Read(bSize));
                            _listener.OnReceive(pack);

                            // SocketPack pack = null;
                            // while ((pack = ParsePack()) != null)
                            // {
                            //     _listener.OnReceive(pack);
                            // }
                        }
                    }
                }
                catch (Exception e)
                {
                    OnException(SocketError.Receive, e);
                    break;
                }
            }
        }

        private SocketPack ParsePack()
        {
            if (!_isBodyParsing)
            {
                if (_receiver.dataSize >= HEAD_SIZE)
                {
                    if (_header.isEmpty())
                    {
                        _header.Write(_receiver.Read(HEAD_SIZE));
                        _isBodyParsing = true;

                        _packSize = _header.ReadInt32();
                        _cmd = _header.ReadInt32();
                        _session = _header.ReadInt64();
                        _packType = _header.ReadInt16();
                    }
                }
            }

            if (_isBodyParsing)
            {
                if (_receiver.dataSize >= _packSize)
                {
                    return null;
                }
            }
            return null;
        }

        public void Disconnecte()
        {
            if (!isConnected)
                return;
            OnDisconnected();
            _listener.OnDisconnected();
        }

        private void OnDisconnected()
        {
            Debug.Log("socket disconnected && closed.");

            isConnected = false;
            isConnecting = false;

            if (_thread != null)
                _thread.Abort();
            _thread = null;

            if (_client != null && _client.Connected)
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
            }
            _client = null;
        }

        private void OnException(SocketError error, Exception e = null)
        {
            _isException = true;
            if (isConnected) OnDisconnected();
            isConnecting = false;
            _listener.OnNetworkError(error, e);

            Debug.LogError(string.Format("socket exception: {0}. {1}", error.ToString(), e != null ? e.ToString() : ""));
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
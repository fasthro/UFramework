/*
 * @Author: fasthro
 * @Date: 2019-08-08 11:33:05
 * @Description: socket client
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UFramework.Network
{
    public enum ProtocalType
    {
        Binary,
        LinearBinary,
        PBC,
        Protobuf,
        Sproto,
    }

    public class SocketClient
    {
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

        /// <summary>
        /// 二进制协议解析
        /// </summary>
        /// <value></value>
        public bool isProtocalBinary
        {
            get { return _isProtocalBinary; }
            set
            {
                _isProtocalBinary = value;

                _sender.Clear();
                _receiver.Clear();
                _sendlenQueue.Clear();
            }
        }

        private Socket _client;
        private Thread _thread;
        private ISocketListener _listener;

        private byte[] _buffer;

        private AutoByteArray _sender;
        private SocketPackHeader _senderHeader;
        private Queue<int> _sendlenQueue;

        private AutoByteArray _receiver;
        private bool _isReceiveHeaderReaded;
        private SocketPackHeader _receiverHeader;

        private int _packSize;
        private int _cmd;
        private long _session;
        private short _packType;

        private bool _isProtocalBinary;
        private float _connectDecreaseTime;
        private int _connectTimeout = 5000;
        private int _receiveBufferSize = 4096;
        private int _sendBufferSize = 4096;

        private bool _isSending;
        private bool _isBodyParsing;
        private bool _isException;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public SocketClient(ISocketListener listener)
        {
            _buffer = new byte[receiveBufferSize];
            _listener = listener;
            _sender = new AutoByteArray(128, 128);
            _receiver = new AutoByteArray(128, 128);
            _senderHeader = new SocketPackHeader();
            _sendlenQueue = new Queue<int>();
            _receiverHeader = new SocketPackHeader();
            _isProtocalBinary = true;
        }

        public void OnUpdate()
        {
            if (isConnecting)
            {
                _connectDecreaseTime -= Time.unscaledDeltaTime;
                if (_connectDecreaseTime <= 0)
                    OnException(new Exception("连接超时"));
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
                OnException(e);
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

                _listener.OnSocketConnected();

                _thread = new Thread(new ThreadStart(OnReceive));
                _thread.IsBackground = true;
                _thread.Start();
            }
            catch (Exception e)
            {
                OnException(e);
            }
        }

        public void Send(SocketPack pack)
        {
            if (isConnected)
            {
                pack.Pack();
                if (pack.protocal != ProtocalType.Binary)
                {
                    _senderHeader.Pack(pack.protocal, pack.cmd, pack.rawDataSize);
                    _sender.Write(_senderHeader.rawData);
                }
                else
                {
                    _sendlenQueue.Enqueue(pack.rawDataSize);
                }
                _sender.Write(pack.rawData);
            }
        }

        private void ProcessSend()
        {
            if (!_isSending)
            {
                try
                {
                    if (!_sender.isEmpty)
                    {
                        _isSending = true;
                        if (isProtocalBinary)
                        {
                            var len = _sendlenQueue.Dequeue();
                            _client.BeginSend(_sender.Read(len), 0, len, SocketFlags.DontRoute, OnSend, null);
                        }
                        else
                        {
                            if (_sender.size > sendBufferSize)
                            {
                                _client.BeginSend(_sender.Read(sendBufferSize), 0, sendBufferSize, SocketFlags.DontRoute, OnSend, null);
                            }
                            else
                            {
                                var len = _sender.size;
                                _client.BeginSend(_sender.ReadAll(), 0, len, SocketFlags.DontRoute, OnSend, null);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                }
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
                OnException(e);
            }
        }

        private void OnReceive()
        {
            while (true)
            {
                try
                {
                    if (_client != null && _client.Connected)
                    {
                        int bSize = _client.Receive(_buffer);
                        if (bSize > 0)
                        {
                            _receiver.Write(_buffer, bSize);

                            SocketPack pack = null;
                            if (isProtocalBinary)
                            {
                                _listener.OnSocketReceive(SocketPack.CreateReader<SocketPackBinary>(-1, _receiver.Read(bSize)));
                            }
                            else
                            {
                                while ((pack = ParsePack()) != null)
                                {
                                    _listener.OnSocketReceive(pack);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    OnException(e);
                }
            }
        }

        private SocketPack ParsePack()
        {
            if (_receiver.size >= SocketPackHeader.SIZE && !_isReceiveHeaderReaded)
            {
                _receiverHeader.Unpack(_receiver.Read(SocketPackHeader.SIZE));
                _isReceiveHeaderReaded = true;
            }
            else if (_isReceiveHeaderReaded && _receiver.size >= _receiverHeader.dataSize)
            {
                var data = _receiver.Read(_receiverHeader.dataSize);
                SocketPack pack = null;
                switch (_receiverHeader.protocal)
                {
                    case ProtocalType.LinearBinary:
                        pack = SocketPack.CreateReader<SocketPackLinearBinary>(_receiverHeader.cmd, data);
                        break;
                    case ProtocalType.PBC:
                        pack = SocketPack.CreateReader<SocketPackPBC>(_receiverHeader.cmd, data);
                        break;
                    case ProtocalType.Protobuf:
                        pack = SocketPack.CreateReader<SocketPackProtobuf>(_receiverHeader.cmd, data);
                        break;
                    case ProtocalType.Sproto:
                        pack = SocketPack.CreateReader<SocketPackSproto>(_receiverHeader.cmd, data);
                        break;
                }
                _isReceiveHeaderReaded = false;
                return pack;
            }
            return null;
        }

        public void Disconnecte()
        {
            if (!isConnected)
                return;
            OnDisconnected();
            _listener.OnSocketDisconnected();
        }

        private void OnDisconnected()
        {
            Debug.Log("socket disconnected && closed.");

            isConnected = false;
            isConnecting = false;

            if (_client != null && _client.Connected)
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
            }
            _client = null;

            if (_thread != null)
                _thread.Abort();
            _thread = null;
        }

        private void OnException(Exception e = null)
        {
            Debug.LogError(string.Format("socket exception: {1}", e != null ? e.ToString() : ""));

            _isException = true;
            if (isConnected) OnDisconnected();
            isConnecting = false;
            _listener.OnSocketException(e);
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
/*
 * @Author: fasthro
 * @Date: 2019-08-08 11:33:05
 * @Description: tcp socket client
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
    public class TcpSocketClient : ISocketClient
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
        /// 协议类型
        /// </summary>
        /// <value></value>
        public PackType packType { get; private set; }

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
        private ISocketClientListener _listener;

        private byte[] _buffer;

        private AutoByteArray _sender;
        private Queue<int> _sendlenQueue;
        private FixedByteArray _sendPackSizer;

        private AutoByteArray _receiver;
        private bool _isReceiveSizeReaded;
        private int _receivePackSize;
        private FixedByteArray _receivePackHeader;
        private FixedByteArray _receivePackSizer;

        private int _packSize;
        private int _cmd;
        private long _session;
        private short _packType;

        private float _connectDecreaseTime;
        private int _connectTimeout = 15000;
        private int _receiveBufferSize = 8192;
        private int _sendBufferSize = 8192;

        private bool _isSending;
        private bool _isBodyParsing;
        private bool _isException;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        /// <param protocal="listener">初始协议类型</param>
        public TcpSocketClient(ISocketClientListener listener, PackType packType = PackType.Binary)
        {
            _buffer = new byte[receiveBufferSize];
            _listener = listener;
            _sender = new AutoByteArray(128, 128);
            _receiver = new AutoByteArray(128, 128);
            _sendlenQueue = new Queue<int>();
            _sendPackSizer = new FixedByteArray(2);
            _receivePackHeader = new FixedByteArray(SocketPack.HEADER_SIZE);
            _receivePackSizer = new FixedByteArray(2);
            this.packType = packType;
        }

        public void Update()
        {
            if (isConnecting)
            {
                _connectDecreaseTime -= Time.unscaledDeltaTime;
                if (_connectDecreaseTime <= 0)
                    OnException(SocketError.TimedOut);
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
                this.ip = Utils.GetIPHost(ip);

            Connect();
        }

        private void Connect()
        {
            if (isConnecting || isConnected) return;

            try
            {
                _isException = false;
                string newIp = "";
                string connetIp = ip;
                // ipv6 & ipv4
                AddressFamily newAddressFamily = AddressFamily.InterNetwork;
                IPv6SupportMidleware.getIPType(ip, port.ToString(), out newIp, out newAddressFamily);
                if (!string.IsNullOrEmpty(newIp)) { connetIp = newIp; }

                Logger.Debug("socket connect to server. " + ip + ":" + port + (string.IsNullOrEmpty(newIp) ? " ipv4" : " ipv6"));

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
            catch (SocketException e)
            {
                OnException(e.SocketErrorCode);
            }
        }

        private void OnConnetSucceed(IAsyncResult iar)
        {
            if (_isException) return;

            try
            {
                Logger.Debug("socket connect to server succeed.");
                ((Socket)iar.AsyncState).EndConnect(iar);
                isConnected = true;
                isConnecting = false;

                _listener.OnSocketConnected();

                _thread = new Thread(new ThreadStart(OnReceive));
                _thread.IsBackground = true;
                _thread.Start();
            }
            catch (SocketException e)
            {
                OnException(e.SocketErrorCode);
            }
        }

        public void Send(SocketPack pack)
        {
            if (packType != pack.packType)
            {
                packType = pack.packType;

                _sender.Clear();
                _receiver.Clear();
                _sendlenQueue.Clear();
            }

            if (isConnected)
            {
                _sendPackSizer.Clear();
                pack.PackSendData();
                switch (pack.packType)
                {
                    case PackType.Binary:
                        _sendlenQueue.Enqueue(pack.rawDataSize);
                        break;
                    case PackType.SizeBinary:
                    case PackType.SizeHeaderBinary:
                        _sender.Write(pack.sizer.buffer);
                        break;
                }

                if (!_sendPackSizer.isEmpty)
                    _sender.Write(_sendPackSizer.buffer);

                if (!pack.header.isEmpty)
                    _sender.Write(pack.header.buffer);

                _sender.Write(pack.rawData);
            }
            pack.Recycle();
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
                        if (packType == PackType.Binary)
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
                                var data = _sender.ReadAll();
                                _client.BeginSend(data, 0, data.Length, SocketFlags.DontRoute, OnSend, null);
                            }
                        }
                    }
                }
                catch (SocketException e)
                {
                    OnException(e.SocketErrorCode);
                }
            }
        }

        private void OnSend(IAsyncResult iar)
        {
            try
            {
                _isSending = false;
            }
            catch (SocketException e)
            {
                OnException(e.SocketErrorCode);
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

                            if (packType == PackType.Binary)
                            {
                                _listener.OnSocketReceive(SocketPack.AllocateReader(packType, _receiver.Read(bSize)));
                            }
                            else
                            {
                                SocketPack pack = null;
                                while ((pack = ParsePack()) != null)
                                {
                                    _listener.OnSocketReceive(pack);
                                }
                            }
                        }
                    }
                }
                catch (ThreadAbortException e)
                { }
                catch (SocketException e)
                {
                    OnException(e.SocketErrorCode);
                }
            }
        }

        private SocketPack ParsePack()
        {
            if (_receiver.size >= 2 && !_isReceiveSizeReaded)
            {
                _receivePackSizer.Clear();
                _receivePackSizer.Write(_receiver.Read(2));

                _receivePackSize = _receivePackSizer.ReadUInt16();
                _isReceiveSizeReaded = true;
            }
            if (_isReceiveSizeReaded && _receiver.size >= _receivePackSize)
            {
                _isReceiveSizeReaded = false;
                if (packType == PackType.SizeHeaderBinary)
                {
                    _receivePackHeader.Clear();
                    _receivePackHeader.Write(_receiver.Read(SocketPack.HEADER_SIZE));
                    return SocketPack.AllocateReader(packType, _receivePackHeader, _receiver.Read(_receivePackSize - SocketPack.HEADER_SIZE));
                }
                return SocketPack.AllocateReader(packType, _receiver.Read(_receivePackSize));
            }
            return null;
        }

        public void Disconnect()
        {
            if (!isConnected)
                return;
            OnDisconnected();
            _listener.OnSocketDisconnected();
        }

        public void Dispose() { }

        private void OnDisconnected()
        {
            Logger.Debug("socket disconnected && closed.");

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

        private void OnException(SocketError error)
        {
            Logger.Error("socket exception: " + error.ToString());

            _isException = true;
            if (isConnected) OnDisconnected();
            isConnecting = false;
            _listener.OnSocketException(error);
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-12-16 16:27:00
 * @Description: udp socket client
 */
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using LiteNetLib.Utils;
using UFramework.Core;

namespace UFramework.Network
{
    public class UdpSocketClient : INetEventListener, ISocketClient
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

        private LiteNetLib.NetManager _client;
        private ISocketClientListener _listener;

        private NetDataWriter _sender;
        private DoubleQueue<SocketPack> _sendQueue;

        private FixedByteArray _fixedHeadReader;
        private byte[] _fixedBuff;

        public UdpSocketClient(ISocketClientListener listener, PackType protocal = PackType.Binary)
        {
            _listener = listener;
            this.packType = protocal;

            _sendQueue = new DoubleQueue<SocketPack>();
            _sender = new NetDataWriter();

            _fixedHeadReader = new FixedByteArray(SocketPack.HEADER_SIZE);
            _fixedBuff = new byte[SocketPack.HEADER_SIZE];

            _client = new LiteNetLib.NetManager(this);
            _client.UpdateTime = 15;
            _client.UnconnectedMessagesEnabled = true;
            _client.Start();
        }

        public void Update()
        {
            if (isConnected)
            {
                ProcessSend();
            }

            _client.PollEvents();
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
            isConnected = false;
            isConnecting = true;
            _client.Connect(ip, port, "UFramework");
        }

        public void Send(SocketPack pack)
        {
            if (packType != pack.packType)
            {
                packType = pack.packType;
                _sendQueue.Clear();
            }

            _sendQueue.Enqueue(pack);
        }

        private void ProcessSend()
        {
            if (_sendQueue.IsEmpty())
            {
                _sendQueue.Swap();
            }
            while (!_sendQueue.IsEmpty())
            {
                _sender.Reset();

                var pack = _sendQueue.Dequeue();
                pack.PackSendData();
                _sender.Put(pack.sizer.buffer);
                if (!pack.header.isEmpty)
                    _sender.Put(pack.header.buffer);

                _sender.Put(pack.rawData);

                var peer = _client.FirstPeer;
                peer?.Send(_sender, DeliveryMethod.ReliableOrdered);

                pack.Recycle();
            }
        }

        public void Disconnect()
        {

        }

        public void Dispose() { }

        public void OnPeerConnected(NetPeer peer)
        {
            isConnecting = false;
            isConnected = true;

            _listener.OnSocketConnected();
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {

        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {

        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (packType == PackType.Binary)
            {
                _listener.OnSocketReceive(SocketPack.AllocateReader(packType, reader.RawData));
            }
            else
            {
                reader.GetUShort();
                if (packType == PackType.SizeHeaderBinary)
                {
                    reader.GetBytes(_fixedBuff, SocketPack.HEADER_SIZE);
                    _fixedHeadReader.Clear();
                    _fixedHeadReader.Write(_fixedBuff);

                    var count = reader.AvailableBytes;
                    byte[] bytes = new byte[count];
                    reader.GetBytes(bytes, count);

                    _listener.OnSocketReceive(SocketPack.AllocateReader(packType, _fixedHeadReader, bytes));
                }
                else
                {
                    var count = reader.AvailableBytes;
                    byte[] bytes = new byte[count];
                    reader.GetBytes(bytes, count);

                    _listener.OnSocketReceive(SocketPack.AllocateReader(packType, bytes));
                }
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {

        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {

        }

        public void OnConnectionRequest(ConnectionRequest request)
        {

        }
    }
}
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

        private AutoByteArray _receiver;
        private FixedByteArray _receivePackSizer;
        private FixedByteArray _receivePackHeader;

        public UdpSocketClient(ISocketClientListener listener, PackType protocal = PackType.Binary)
        {
            _listener = listener;
            this.packType = protocal;

            _sendQueue = new DoubleQueue<SocketPack>();
            _sender = new NetDataWriter();
            _receiver = new AutoByteArray(128);
            _receivePackSizer = new FixedByteArray(2);
            _receivePackHeader = new FixedByteArray(SocketPack.HEADER_SIZE);

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
                var pack = _sendQueue.Dequeue();

                pack.PackSendData();
                switch (pack.packType)
                {
                    case PackType.SizeBinary:
                        _sender.Put((ushort)pack.rawDataSize);
                        break;
                    case PackType.SizeHeaderBinary:
                        _sender.Put((ushort)(SocketPack.HEADER_SIZE + pack.rawDataSize));
                        break;
                }

                if (!pack.header.isEmpty)
                    _sender.Put(pack.header.buffer);

                _sender.Put(pack.rawData);

                var peer = _client.FirstPeer;
                peer.Send(_sender, DeliveryMethod.ReliableOrdered);

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
                _receiver.Clear();
                _receiver.Write(reader.RawData);

                _receivePackSizer.Clear();
                _receivePackSizer.Write(_receiver.Read(2));

                var packSize = _receivePackSizer.ReadUInt16();

                if (packType == PackType.SizeHeaderBinary)
                {
                    _receivePackHeader.Clear();
                    _receivePackHeader.Write(_receiver.Read(SocketPack.HEADER_SIZE));
                    _listener.OnSocketReceive(SocketPack.AllocateReader(packType, _receivePackHeader, _receiver.Read(packSize - SocketPack.HEADER_SIZE)));
                }
                _listener.OnSocketReceive(SocketPack.AllocateReader(packType, _receiver.Read(packSize)));
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
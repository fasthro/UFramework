/*
 * @Author: fasthro
 * @Date: 2020-12-16 17:36:49
 * @Description: socekt channel
 */
using System;
using System.Net.Sockets;
using UFramework.Core;

namespace UFramework.Network
{
    public enum ProtocalType
    {
        Tcp,
        Udp
    }

    public class SocketChannel : ISocketClientListener, ISocketClient
    {
        public int channelId { get; private set; }
        public bool isConnected => _client.isConnected;
        public bool isRedirecting { get; private set; }
        public ProtocalType protocalType { get; private set; }
        public PackType packType { get; private set; }

        private ISocketChannelListener _listener;
        private ISocketClient _client;

        public readonly DoubleQueue<SocketPack> packQueue = new DoubleQueue<SocketPack>();

        public SocketChannel(int channelId, ProtocalType protocalType, ISocketChannelListener listener, PackType packType = PackType.Binary)
        {
            this.channelId = channelId;
            this.protocalType = protocalType;
            this.packType = packType;
            this._listener = listener;

            Initialize();
        }

        private void Initialize()
        {
            packQueue.Clear();
            switch (protocalType)
            {
                case ProtocalType.Tcp:
                    _client = (new TcpSocketClient(this, packType)) as ISocketClient;
                    break;
                case ProtocalType.Udp:
                    _client = (new UdpSocketClient(this, packType)) as ISocketClient;
                    break;
            }
        }

        public void OnSocketConnected()
        {
            isRedirecting = false;
            _listener.OnSocketChannelConnected(channelId);
        }

        public void OnSocketDisconnected()
        {
            isRedirecting = false;
            _listener.OnSocketChannelDisconnected(channelId);
        }

        public void OnSocketReceive(SocketPack pack)
        {
            packQueue.Enqueue(pack);
            _listener.OnSocketChannelReceive(channelId);
        }

        public void OnSocketException(SocketError error)
        {
            isRedirecting = false;
            _listener.OnSocketChannelException(channelId, error);
        }

        #region ISocketClient

        public void Connect(string ip, int port)
        {
            if (!isConnected)
                _client.Connect(ip, port);
        }

        public void Send(SocketPack pack)
        {
            if (isConnected)
                _client.Send(pack);
        }

        public void Disconnect()
        {
            if (isConnected)
                _client.Disconnect();
        }

        public void Redirect(string ip, int port)
        {
            isRedirecting = true;
            _client?.Dispose();
            Initialize();
            Connect(ip, port);
        }

        public void Update()
        {
            _client.Update();
        }

        public void Dispose()
        {
            _client.Dispose();
            _client = null;
        }

        #endregion
    }
}
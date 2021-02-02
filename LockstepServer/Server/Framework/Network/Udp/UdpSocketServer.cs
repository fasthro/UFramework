// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using LiteNetLib;
using System.Net;
using System.Net.Sockets;

namespace UFramework
{
    public class UdpSocketServer : INetEventListener
    {
        public bool isRunning { get; private set; }

        private IServerListener _listener;
        private LiteNetLib.NetManager _server;

        public UdpSocketServer(IServerListener listener)
        {
            _listener = listener;
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (_server.GetPeersCount(ConnectionState.Connected) < 10)
            {
                request.AcceptIfKey("UFramework");
            }
            else
            {
                // 拒绝掉链接
                request.Reject();
            }

            LogHelper.Info("客户端连接请求 [" + request.RemoteEndPoint + "]");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            LogHelper.Error("网络异常 [" + socketError + "]");
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            _listener.OnNetworkReceive(peer, reader, deliveryMethod);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.BasicMessage)
            {
                LogHelper.Info("Received discovery request. Send discovery reOnNetworkLatencyUpdatesponse");
            }
        }

        public void OnPeerConnected(NetPeer peer)
        {
            _listener.OnPeerConnected(peer);
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            _listener.OnPeerDisconnected(peer, disconnectInfo);
        }

        public void StartServer(int port)
        {
            isRunning = true;
            _server = new LiteNetLib.NetManager(this) {UpdateTime = 15};
            _server.Start(port);
        }

        public void StopServer()
        {
            if (_server != null)
            {
                _server.Stop();
                _server = null;
            }

            isRunning = false;
        }

        public void Update()
        {
            _server?.PollEvents();
        }
    }
}
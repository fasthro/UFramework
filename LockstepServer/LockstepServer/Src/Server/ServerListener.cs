using LiteNetLib;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LockstepServer
{
    public class ServerListener : INetEventListener
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(ServerListener));

        public bool isRunning { get; private set; }

        private NetManager _server;

        public void StartServer(int port)
        {
            isRunning = true;
            _server = new NetManager(this);
            _server.UpdateTime = 15;
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
            if (_server != null)
            {
                _server.PollEvents();
            }
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
            logger.Info("[SERVER] 客户端连接请求 [" + request.RemoteEndPoint + "]");
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            logger.Error("[SERVER] 网络异常 [" + socketError + "]");
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            if (messageType == UnconnectedMessageType.BasicMessage)
            {
                logger.Warn("[SERVER] Received discovery request. Send discovery reOnNetworkLatencyUpdatesponse");
            }
        }

        public void OnPeerConnected(NetPeer peer)
        {
            logger.Warn("[SERVER] 客户端连接成功 [" + peer.EndPoint + "]");
            Console.WriteLine("[SERVER] 客户端连接成功 [" + peer.EndPoint + "]");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            logger.Warn("[SERVER] 客户端连接断开 [" + peer.EndPoint + "]");
        }
    }
}

using System;

namespace UFramework.Network
{
    public interface ISocketListener
    {
        void OnConnected();
        void OnDisconnected();
        void OnReceive(SocketPack pack);
        void OnNetworkError(SocketError code, Exception error);
    }
}
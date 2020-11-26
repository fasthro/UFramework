using System;

namespace UFramework.Network
{
    public interface ISocketListener
    {
        void OnSocketConnected();
        void OnSocketDisconnected();
        void OnSocketReceive(SocketPack pack);
        void OnSocketException(Exception exception);
    }
}
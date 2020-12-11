using System;

namespace UFramework.Core
{
    public interface ISocketListener
    {
        void OnSocketConnected();
        void OnSocketDisconnected();
        void OnSocketReceive(SocketPack pack);
        void OnSocketException(Exception exception);
    }
}
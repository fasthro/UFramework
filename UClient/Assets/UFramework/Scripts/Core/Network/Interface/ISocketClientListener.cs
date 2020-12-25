/*
 * @Author: fasthro
 * @Date: 2020-12-01 16:51:34
 * @Description: 
 */
using System;
using System.Net.Sockets;

namespace UFramework.Network
{
    public interface ISocketClientListener
    {
        void OnSocketConnected();
        void OnSocketDisconnected();
        void OnSocketReceive(SocketPack pack);
        void OnSocketException(SocketError error);
    }
}
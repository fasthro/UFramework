/*
 * @Author: fasthro
 * @Date: 2020-12-01 16:51:34
 * @Description: 
 */
using System;
using System.Net.Sockets;

namespace UFramework.Network
{
    public interface ISocketChannelListener
    {
        void OnSocketChannelConnected(int channelId);
        void OnSocketChannelDisconnected(int channelId);
        void OnSocketChannelReceive(int channelId);
        void OnSocketChannelException(int channelId, SocketError error);
    }
}
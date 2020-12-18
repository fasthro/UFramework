/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:28:02
 * @Description: 网络管理
 */

using LiteNetLib;
using System;

namespace LockstepServer
{
    internal class NetManager : BaseManager, IServerListener
    {
        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
        }

        public void OnPeerConnected(NetPeer peer)
        {
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:44:00
 * @Description:
 */

using LiteNetLib;

namespace LockstepServer
{
    public interface IServerListener
    {
        void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod);

        void OnPeerConnected(NetPeer peer);

        void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo);
    }
}
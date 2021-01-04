/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:49:52
 * @Description:
 */

using Google.Protobuf;
using LiteNetLib;

namespace LockstepServer
{
    public interface INetworkService : IService
    {
        void Send(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, IMessage message);
    }
}
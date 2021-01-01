/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:49:52
 * @Description:
 */

using LiteNetLib;

namespace LockstepServer
{
    public interface IHandlerService : IService
    {
        void OnReceive(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, byte[] data);
    }
}
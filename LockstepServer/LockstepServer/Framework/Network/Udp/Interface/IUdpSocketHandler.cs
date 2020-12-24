/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:22:02
 * @Description:
 */

using LiteNetLib;

namespace LockstepServer
{
    public interface IUdpSocketHandler
    {
        NetPeer peer { get; set; }

        void DoMessage(NetPeer peer, int session, NetworkProcessLayer layer, byte[] bytes);
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using LiteNetLib;

namespace UFramework
{
    public interface IUdpSocketHandler
    {
        NetPeer peer { get; set; }

        void DoMessage(NetPeer peer, int session, NetworkProcessLayer layer, byte[] bytes);
    }
}
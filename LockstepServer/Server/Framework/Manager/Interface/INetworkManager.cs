// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Google.Protobuf;
using LiteNetLib;

namespace UFramework
{
    public interface INetworkManager : IManager
    {
        void Send(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, IMessage message);
    }
}
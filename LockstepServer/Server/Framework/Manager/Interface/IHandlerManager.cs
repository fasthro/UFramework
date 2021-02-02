// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using LiteNetLib;

namespace UFramework
{
    public interface IHandlerManager : IManager
    {
        void RegisterHandler(int protocal, IHandler handler);

        void OnReceive(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, byte[] data);
    }
}
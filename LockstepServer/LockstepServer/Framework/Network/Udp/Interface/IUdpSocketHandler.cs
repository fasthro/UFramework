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
        void OnMessage(NetPeer peer, int session, byte[] bytes);
    }
}
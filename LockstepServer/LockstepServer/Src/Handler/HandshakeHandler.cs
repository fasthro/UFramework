/*
 * @Author: fasthro
 * @Date: 2020/12/22 15:59:29
 * @Description: 握手验证
 */

using Google.Protobuf;
using LiteNetLib;
using PBBattleServer;

namespace LockstepServer.Src
{
    public class HandshakeHandler : IHandler
    {
        public int cmd => 901;

        public void OnMessage(NetPeer peer, int session, byte[] bytes)
        {
            Handshake_C2S handshake = Handshake_C2S.Parser.ParseFrom(bytes);
            LogHelper.Info($"客户端握手验证[{handshake.Secret}]");

            Handshake_S2C s2c = new Handshake_S2C();
            s2c.ResultCode = ResultCode.SUCCEED;
            Service.Instance.GetManager<NetManager>().Send(peer, cmd, session, s2c);
        }
    }
}
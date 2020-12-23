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
            var playerManager = Service.Instance.GetManager<PlayerManager>();
            var modelManager = Service.Instance.GetManager<ModelManager>();
            var roomManager = Service.Instance.GetManager<RoomManager>();

            #region rec

            Handshake_C2S c2s = Handshake_C2S.Parser.ParseFrom(bytes);
            LogHelper.Info($"客户端握手验证[{c2s.Uid}]");

            if (!playerManager.ExistPlayer(c2s.Uid))
            {
                var userModel = modelManager.GetModel<UserModel>();
                if (userModel != null)
                {
                    if (userModel.ExistUser(c2s.Uid) == 0L)
                    {
                        UserInfo user = new UserInfo();
                        user.uid = c2s.Uid;
                        userModel.AddUser(user);
                    }
                }

                playerManager.AddPlayer(new Player(c2s.Uid, new UdpSession(peer)));
            }

            #endregion rec

            #region send

            Handshake_S2C s2c = new Handshake_S2C();
            s2c.ResultCode = ResultCode.SUCCEED;
            s2c.RoomSecretKey = roomManager.room.secretKey;
            Service.Instance.GetManager<NetManager>().Send(peer, cmd, session, s2c);

            #endregion send
        }
    }
}
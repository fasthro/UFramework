/*
 * @Author: fasthro
 * @Date: 2020/12/22 15:59:29
 * @Description: 握手验证
 */

using Google.Protobuf;
using LiteNetLib;
using PBBS;

namespace LockstepServer.Src
{
    public class HandshakeHandler : BaseGameHandler
    {
        public override int cmd => 901;

        protected override void OnMessage(byte[] bytes)
        {
            Handshake_C2S c2s = Handshake_C2S.Parser.ParseFrom(bytes);
            LogHelper.Info($"客户端握手验证[{c2s.Uid}]");

            if (!_playerService.ExistPlayer(c2s.Uid))
            {
                var userModel = _modelService.GetModel<UserModel>();
                if (userModel != null)
                {
                    if (userModel.ExistUser(c2s.Uid) == 0L)
                    {
                        UserInfo user = new UserInfo();
                        user.uid = c2s.Uid;
                        userModel.AddUser(user);
                    }
                }
                _playerService.AddPlayer(new Player(c2s.Uid, new UdpSession(peer)));
            }
        }

        protected override bool OnResponse()
        {
            Handshake_S2C s2c = CreateResponseMessage<Handshake_S2C>();
            s2c.ResultCode = ResultCode.SUCCEED;
            s2c.RoomSecretKey = _roomService.room.secretKey;
            return true;
        }
    }
}
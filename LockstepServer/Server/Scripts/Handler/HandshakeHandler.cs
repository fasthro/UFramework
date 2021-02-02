// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep;
using Lockstep.Message;
using MongoDB.Bson;
using UFramework;

namespace GameServer
{
    public class HandshakeHandler : BaseGameHandler
    {
        public override int cmd => 901;

        protected override void OnMessage(byte[] bytes)
        {
            var c2s = Handshake_C2S.Parser.ParseFrom(bytes);
            LogHelper.Info($"Handshake UID [{c2s.Uid}]");

            if (!_playerManager.ExistPlayer(c2s.Uid))
            {
                var userModel = _modelManager.GetModel<UserModel>();
                
                // TODO
                if (userModel.ExistUser(c2s.Uid) == 0L)
                    userModel.AddUser(new UserInfo() {Id = ObjectId.GenerateNewId(), uid = c2s.Uid, password = "fasthro", username = "fasehro"});
                
                var user = userModel.GetUser(c2s.Uid);
                if (user != null)
                {
                    _playerManager.AddPlayer(new Player(user, new UdpSession(peer)));
                }
                else
                {
                    LogHelper.Error($"user don't exist. uid: [{c2s.Uid}]");
                }
            }
        }

        protected override bool OnResponse()
        {
            CreateResponseMessage<Handshake_S2C>();
            return true;
        }
    }
}
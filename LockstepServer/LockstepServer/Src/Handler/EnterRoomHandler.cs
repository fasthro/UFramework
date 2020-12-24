/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:23:57
 * @Description: 进入房间
 */

using PBBattleServer;

namespace LockstepServer.Src
{
    public class EnterRoomHandler : BaseHandler
    {
        private bool _enterSucceed;

        public override int cmd => 902;
        public PlayerManager playerManager => Service.Instance.GetManager<PlayerManager>();
        public RoomManager roomManager => Service.Instance.GetManager<RoomManager>();

        protected override void OnMessage(byte[] bytes)
        {
            EnterRoom_C2S c2s = EnterRoom_C2S.Parser.ParseFrom(bytes);
            LogHelper.Info($"客户端进入房间请求[{c2s.RoomSecretKey}]");

            _enterSucceed = false;
            var player = playerManager.GetPlayer(peer.Id);
            if (player != null)
            {
                _enterSucceed = roomManager.EnterRoom(player, c2s.RoomSecretKey);
            }
        }

        protected override bool OnResponse()
        {
            EnterRoom_S2C s2c = CreateResponseMessage<EnterRoom_S2C>();
            s2c.ResultCode = _enterSucceed ? ResultCode.SUCCEED : ResultCode.FAILED;
            return true;
        }

        protected override void OnAction()
        {
            if (_enterSucceed)
            {
                if (roomManager.room.status == RoomStatus.Ready)
                {
                    roomManager.room.Start();
                }
            }
        }
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep.Message;
using UFramework;

namespace GameServer
{
    public class ReadyHandler : BaseGameHandler
    {
        public override int cmd => 903;

        protected override void OnMessage(byte[] bytes)
        {
            var player = _playerManager.GetPlayer(peer.Id);
            if (player != null && player.insideRoom)
            {
                player.ReadyGame();
            }
        }

        protected override bool OnResponse()
        {
            CreateResponseMessage<Ready_S2C>();
            return true;
        }

        protected override void OnAction()
        {
            var player = _playerManager.GetPlayer(peer.Id);
            if (player != null && player.insideRoom)
            {
                _roomManager.RoomTryStartGame(player.roomId);
            }
        }
    }
}
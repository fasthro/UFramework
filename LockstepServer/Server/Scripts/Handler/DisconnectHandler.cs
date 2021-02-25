// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using UFramework;

namespace GameServer
{
    public class DisconnectHandler : BaseGameHandler
    {
        public override int cmd => GameConst.NETWORK_CMD_CLIENT_DISCONNECT;

        protected override void OnMessage(byte[] bytes)
        {
            var player = _playerManager.GetPlayer(peer.Id);
            if (player != null)
            {
                if (player.roomId != -1)
                {
                    _roomManager.LeaveRoom(player.uid, player.roomId);
                }
                _playerManager.RemovePlayer(player.uid);
            }
        }

        protected override bool OnResponse()
        {
            return false;
        }
    }
}
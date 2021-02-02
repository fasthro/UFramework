// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep.Message;
using UFramework;

namespace GameServer
{
    public class EnterRoomHandler : BaseGameHandler
    {
        public override int cmd => 902;
        
        protected override void OnMessage(byte[] bytes)
        {
            var player = _playerManager.GetPlayer(peer.Id);
            if (player != null)
                _roomManager.EnterRoom(player);
        }

        protected override bool OnResponse()
        {
            CreateResponseMessage<EnterRoom_S2C>();
            return true;
        }
    }
}
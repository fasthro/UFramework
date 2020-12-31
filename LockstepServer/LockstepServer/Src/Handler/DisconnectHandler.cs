/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:24:51
 * @Description:
 */

namespace LockstepServer.Src
{
    public class DisconnectHandler : BaseHandler
    {
        public DisconnectHandler(ServiceContainer container) : base(container)
        {
        }

        public override int cmd => NetwokCmd.CLIENT_DISCONNECT;

        protected override void OnMessage(byte[] bytes)
        {
            var player = _playerService.GetPlayer(peer.Id);
            if (player != null)
            {
                _roomService.room.Remove(player.uid);
            }
        }

        protected override bool OnResponse()
        {
            return false;
        }
    }
}
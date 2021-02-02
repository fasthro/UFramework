// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep;
using Lockstep.Message;
using UFramework;

namespace GameServer
{
    public class FrameHandler : BaseGameHandler
    {
        public override int cmd => NetwokCmd.AGENT_FRAME;

        protected override void OnMessage(byte[] bytes)
        {
            var c2s = Frame_C2S.Parser.ParseFrom(bytes);
            var player = _playerManager.GetPlayer(peer.Id);
            if (player != null && player.insideRoom)
                _roomManager.OnReceiveFrame(player, c2s.Frame.ToFrameData());
        }

        protected override bool OnResponse()
        {
            return false;
        }
    }
}
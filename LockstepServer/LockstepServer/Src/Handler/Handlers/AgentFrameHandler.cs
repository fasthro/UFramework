/*
 * @Author: fasthro
 * @Date: 2021/1/6 12:32:13
 * @Description:
 */

using PBBS;

namespace LockstepServer.Src
{
    public class AgentFrameHandler : BaseGameHandler
    {
        public override int cmd => NetwokCmd.AGENT_FRAME;

        protected override void OnMessage(byte[] bytes)
        {
            Frame_C2S c2s = Frame_C2S.Parser.ParseFrom(bytes);
            _roomService.room.ReceiveAgentFrame(c2s);
        }

        protected override bool OnResponse()
        {
            return false;
        }
    }
}
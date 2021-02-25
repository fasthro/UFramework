// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/23 16:08
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep;
using Lockstep.Message;
using UFramework;

namespace GameServer
{
    public class PingHandler : BaseGameHandler
    {
        public override int cmd => NetworkCmd.PING;
        private long _sendTimestamp;

        protected override void OnMessage(byte[] bytes)
        {
            var c2s = Ping_C2S.Parser.ParseFrom(bytes);
            _sendTimestamp = c2s.SendTimestamp;
        }

        protected override bool OnResponse()
        {
            var s2c = CreateResponseMessage<Ping_S2C>();
            s2c.SendTimestamp = _sendTimestamp;
            s2c.TimeSinceServerStart = LSTime.realtimeSinceStartupMS;
            return true;
        }
    }
}
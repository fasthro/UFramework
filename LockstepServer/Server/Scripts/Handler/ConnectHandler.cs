// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using UFramework;

namespace GameServer
{
    public class ConnectHandler : BaseGameHandler
    {
        public override int cmd => GameConst.NETWORK_CMD_CLIENT_CONNECT;

        protected override void OnMessage(byte[] bytes)
        {
        }

        protected override bool OnResponse()
        {
            return false;
        }
    }
}
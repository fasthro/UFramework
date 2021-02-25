// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/23 11:43
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public class PingMessage
    {
        /// <summary>
        /// 发送时间
        /// </summary>
        public long sendTimestamp;
        
        /// <summary>
        /// 服务器游戏开始时长
        /// </summary>
        public long timeSinceServerStart;
    }
}
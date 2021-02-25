// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework
{
    public static class GameConst
    {
        /// <summary>
        /// 网络数据小端字节序解析
        /// </summary>
        public const bool LITTLE_ENDIAN = false;
        
        /// <summary>
        /// 连接
        /// </summary>
        public const int NETWORK_CMD_CLIENT_CONNECT = -1;

        /// <summary>
        /// 断开连接
        /// </summary>
        public const int NETWORK_CMD_CLIENT_DISCONNECT = -2;

        /// <summary>
        /// 服务器异常
        /// </summary>
        public const int NETWORK_CMD_SERVER_EXCEPTION = 101;

        public const string DB_URL = "mongodb://localhost:27017";
        public const string DB_NAME = "uframework";
    }
}
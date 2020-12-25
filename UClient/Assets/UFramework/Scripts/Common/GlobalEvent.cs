/*
 * @Author: fasthro
 * @Date: 2020-12-24 11:06:55
 * @Description: 全局事件（仅CSharp层使用）
 */
namespace UFramework
{
    public static class GlobalEvent
    {
        #region network

        /// <summary>
        /// 网络连接成功
        /// </summary>
        public const int NET_CONNECTED = 0;

        /// <summary>
        /// 网络已断开
        /// </summary>
        public const int NET_DISCONNECTED = 1;

        /// <summary>
        /// 接受网络协议数据
        /// </summary>
        public const int NET_RECEIVED = 2;

        /// <summary>
        /// 网络异常
        /// </summary>
        public const int NET_EXCEPTION = 3;

        #endregion
    }
}
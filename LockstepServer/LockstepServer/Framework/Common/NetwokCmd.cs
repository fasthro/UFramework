/*
 * @Author: fasthro
 * @Date: 2020/12/24 17:09:18
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public static class NetwokCmd
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        public const int CLIENT_CONNECT = -1;

        /// <summary>
        /// 断开连接
        /// </summary>
        public const int CLIENT_DISCONNECT = -2;

        /// <summary>
        /// 服务器异常
        /// </summary>
        public const int SERVER_EXCEPTION = 101;
    }
}
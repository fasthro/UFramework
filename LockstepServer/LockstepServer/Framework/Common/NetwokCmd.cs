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
        #region base

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

        #endregion base

        #region game

        public const int START = 950;
        public const int PUSH_FRAME = 951;

        #endregion game
    }
}
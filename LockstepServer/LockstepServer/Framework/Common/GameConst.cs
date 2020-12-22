/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:49:00
 * @Description: 常量参数定义
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public static class GameConst
    {
        /// <summary>
        /// 网络数据小端字节序解析
        /// </summary>
        public const bool LITTLE_ENDIAN = false;

        public const string DB_URL = "mongodb://localhost:27017";
        public const string DB_NAME = "uframework";
    }
}
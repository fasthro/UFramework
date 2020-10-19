/*
 * @Author: fasthro
 * @Date: 2020-10-19 11:52:13
 * @Description: time utils
 */
using System;

namespace UFramework
{
    public class TimeUtils
    {
        /// <summary>
        /// UTC - 时间戳
        /// </summary>
        /// <returns></returns>
        public static long UTCTimeStamps()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
    }
}
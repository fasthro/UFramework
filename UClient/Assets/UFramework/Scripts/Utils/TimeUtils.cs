/*
 * @Author: fasthro
 * @Date: 2020-10-19 11:52:13
 * @Description: time utils
 */
using System;
using UnityEngine;

namespace UFramework
{
    public class TimeUtils
    {
        static DateTime standard = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// UTC - 时间戳(毫秒)
        /// </summary>
        /// <returns></returns>
        public static long UTCTimeStamps()
        {
            TimeSpan ts = DateTime.UtcNow - standard;
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// UTC - 时间戳转时间格式
        /// </summary>
        /// <param name="timeStamp">时间戳（毫秒）</param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string UTCTimeStampsFormat(long timeStamp, string format)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(standard).AddMilliseconds(timeStamp).ToString(format);
        }
    }
}
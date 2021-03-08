/*
 * @Author: fasthro
 * @Date: 2019-11-11 14:51:23
 * @Description: string 扩展
 */
using System.Linq;

namespace UFramework
{
    public static class StringExtension
    {
        /// <summary>
        /// 首字母小写写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }
        
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="formatString"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Fmt(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
    }
}
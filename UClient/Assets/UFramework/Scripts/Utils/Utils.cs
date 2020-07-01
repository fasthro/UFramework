/*
 * @Author: fasthro
 * @Date: 2020-07-01 08:59:25
 * @Description: Utils
 */ 
namespace UFramework
{
    public static class Utils
    {

        /// <summary>
        /// Format Bytes String
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytes(long bytes)
        {
            string[] Suffix = { "Byte", "KB", "MB", "GB", "TB" };
            int i = 0;
            double dblSByte = bytes;
            if (bytes > 1024)
                for (i = 0; (bytes / 1024) > 0; i++, bytes /= 1024)
                    dblSByte = bytes / 1024.0;
            return string.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

    }
}
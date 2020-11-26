/*
 * @Author: fasthro
 * @Date: 2020-11-20 13:47:55
 * @Description: 原始字节包
 */
using System.Text;

namespace UFramework.Network
{
    public class SocketPackRawByte : SocketPackLinear
    {
        public SocketPackRawByte() : base()
        {
        }

        public SocketPackRawByte(byte[] data) : base(data)
        {
            rawData = data;
        }

        /// <summary>
        /// 解析成字符串
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string ParseString(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetString(rawData);
        }
    }
}
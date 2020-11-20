/*
 * @Author: fasthro
 * @Date: 2020-11-20 13:47:55
 * @Description: socket pack stream
 */
using System.Text;

namespace UFramework.Network
{
    public class SocketPackStream : SocketPackLine
    {
        public SocketPackStream() : base()
        {
        }

        public SocketPackStream(byte[] data) : base(data)
        {
            rawData = data;
        }

        public string GetString()
        {
            return Encoding.UTF8.GetString(rawData);
        }
    }
}
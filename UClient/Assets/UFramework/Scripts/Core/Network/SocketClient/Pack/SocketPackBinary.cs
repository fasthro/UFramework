/*
 * @Author: fasthro
 * @Date: 2020-11-20 13:47:55
 * @Description: 原始字节包
 */
using UnityEngine;
using System.Text;

namespace UFramework.Network
{
    public class SocketPackBinary : SocketPack
    {
        public override ProtocalType protocal { get { return ProtocalType.Binary; } }

        public string ReadString(Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetString(rawData);
        }
    }
}
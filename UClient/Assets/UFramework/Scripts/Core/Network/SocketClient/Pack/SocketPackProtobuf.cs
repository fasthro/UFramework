/*
 * @Author: fasthro
 * @Date: 2020-11-09 11:22:41
 * @Description: google protobuf
 */
using Google.Protobuf;
using LuaInterface;

namespace UFramework.Network
{
    public class SocketPackProtobuf : SocketPack
    {
        public override ProtocalType protocal { get { return ProtocalType.Protobuf; } }
    }
}
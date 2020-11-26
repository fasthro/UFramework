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
        private IMessage _message;
        private LuaByteBuffer _rawLuaData;

        public SocketPackProtobuf(IMessage message)
        {
            rawData = message.ToByteArray();
        }

        public SocketPackProtobuf(byte[] data)
        {
            rawData = data;
        }

        public SocketPackProtobuf(LuaByteBuffer data)
        {
            rawData = data.buffer;
        }

        public T GetMessage<T>() where T : class, IMessage, new()
        {
            if (_message == null)
            {
                _message = new T();
                _message.MergeFrom(rawData);
            }
            return _message as T;
        }

        public LuaByteBuffer GetRawLuaData()
        {
            if (_rawLuaData.Length == 0)
            {
                _rawLuaData = new LuaByteBuffer(rawData);
            }
            return _rawLuaData;
        }
    }
}
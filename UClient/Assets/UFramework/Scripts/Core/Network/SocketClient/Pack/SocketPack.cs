/*
 * @Author: fasthro
 * @Date: 2020-11-09 10:44:57
 * @Description: socket pack
 */
using System.Text;
using LuaInterface;

namespace UFramework.Network
{
    public class SocketPackHeader
    {
        public readonly static int SIZE = 13;
        static int SESSION = 0;

        /// <summary>
        /// 数据
        /// </summary>
        /// <value></value>
        public byte[] rawData
        {
            get { return _data.data; }
        }

        /// <summary>
        /// 数据长度
        /// </summary>
        /// <value></value>
        public int dataSize
        {
            get { return size - SIZE; }
        }

        /// <summary>
        /// 包长度
        /// </summary>
        /// <value></value>
        public int size { get; private set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        /// <value></value>
        public ProtocalType protocal { get; private set; }

        /// <summary>
        /// 协议号
        /// </summary>
        /// <value></value>
        public int cmd { get; private set; }

        /// <summary>
        /// session
        /// </summary>
        /// <value></value>
        public int session { get; private set; }

        private FixedByteArray _data;

        public SocketPackHeader()
        {
            _data = new FixedByteArray(SIZE);
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="protocal"></param>
        /// <param name="cmd"></param>
        /// <param name="dataSize"></param>
        public void Pack(ProtocalType protocal, int cmd, int dataSize)
        {
            this.size = dataSize + SIZE;
            this.protocal = protocal;
            this.cmd = cmd;
            this.session = SESSION++;

            _data.Clear();
            _data.Write(size);
            _data.Write((byte)protocal);
            _data.Write(cmd);
            _data.Write(session);
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="data"></param>
        public void Unpack(byte[] value)
        {
            _data.Clear();
            _data.Write(value);
            this.size = _data.ReadInt();
            this.protocal = (ProtocalType)_data.ReadByte();
            this.cmd = _data.ReadInt();
            this.session = _data.ReadInt();
        }
    }

    public abstract class SocketPack : ISocketPack
    {
        /// <summary>
        /// 命令
        /// </summary>
        /// <value></value>
        public int cmd { get; protected set; }

        /// <summary>
        /// 数据
        /// </summary>
        /// <value></value>
        [NoToLua]
        public byte[] rawData { get; protected set; }

        /// <summary>
        /// lua 中访问的数据
        /// </summary>
        /// <value></value>
        public LuaByteBuffer luaRawData { get { return new LuaByteBuffer(rawData); } }

        /// <summary>
        /// 数据长度
        /// </summary>
        /// <value></value>
        public int rawDataSize { get { return rawData.Length; } }

        /// <summary>
        /// 协议类型
        /// </summary>
        /// <value></value>
        public abstract ProtocalType protocal { get; }

        public static T CreateReader<T>(int cmd, byte[] data) where T : SocketPack, new()
        {
            var obj = new T();
            obj.InitializeReader(cmd, data);
            return obj;
        }

        public static T CreateWriter<T>(int cmd) where T : SocketPack, new()
        {
            var obj = new T();
            obj.InitializeWriter(cmd);
            return obj;
        }

        protected virtual void InitializeReader(int cmd, byte[] data)
        {
            this.cmd = cmd;
            this.rawData = data;
        }

        protected virtual void InitializeWriter(int cmd)
        {
            this.cmd = cmd;
        }

        [NoToLua]
        public virtual void WriteBuffer(byte[] value)
        {
            rawData = value;
        }

        [NoToLua]
        public virtual void WriteBuffer(string value, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            rawData = encoding.GetBytes(value);
        }

        public virtual void WriteBuffer(LuaByteBuffer value)
        {
            rawData = value.buffer;
        }

        [NoToLua]
        public virtual void Pack()
        {

        }

        [NoToLua]
        public virtual void Unpack()
        {

        }

        /// <summary>
        /// 转换对象类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToPack<T>() where T : SocketPack
        {
            return (T)this;
        }
    }
}
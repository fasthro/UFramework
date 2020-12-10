/*
 * @Author: fasthro
 * @Date: 2020-11-09 10:44:57
 * @Description: socket pack
 */
using System.Text;
using LuaInterface;
using UFramework.Pool;

namespace UFramework.Network
{
    public class SocketPack : IPoolObject
    {
        /// <summary>
        /// 头长度
        /// byte[] [0-3]:cmd [4-7]:session
        /// </summary>
        public readonly static int HEADER_SIZE = 8;
        static int SESSION = 0;

        /// <summary>
        /// 命令
        /// </summary>
        /// <value></value>
        public int cmd { get; protected set; }

        /// <summary>
        /// session
        /// </summary>
        /// <value></value>
        public int session { get; private set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        /// <value></value>
        public ProtocalType protocal { get; private set; }

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
        /// header bytes
        /// </summary>
        /// <value></value>
        public FixedByteArray header { get; private set; }

        #region pool

        public bool isRecycled { get; set; }

        public static SocketPack AllocateReader(ProtocalType protocal, byte[] data)
        {
            return ObjectPool<SocketPack>.Instance.Allocate().InitializeReader(protocal, null, data);
        }

        public static SocketPack AllocateReader(ProtocalType protocal, FixedByteArray header, byte[] data)
        {
            return ObjectPool<SocketPack>.Instance.Allocate().InitializeReader(protocal, header, data);
        }

        public static SocketPack AllocateWriter(ProtocalType protocal, int cmd)
        {
            return ObjectPool<SocketPack>.Instance.Allocate().InitializeWriter(protocal, cmd);
        }

        public void Recycle()
        {
            ObjectPool<SocketPack>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }

        #endregion

        public SocketPack()
        {
            header = new FixedByteArray(HEADER_SIZE);
        }

        private SocketPack InitializeReader(ProtocalType protocal, FixedByteArray fixHeader, byte[] data)
        {
            this.protocal = protocal;
            this.rawData = data;
            this.header.Clear();
            if (fixHeader != null)
            {
                this.header.Write(fixHeader.buffer);
            }
            return this;
        }

        private SocketPack InitializeWriter(ProtocalType protocal, int cmd)
        {
            this.protocal = protocal;
            this.cmd = cmd;
            this.session = SESSION++;
            return this;
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

        public void PackSendData()
        {
            if (protocal == ProtocalType.SizeHeaderBinary)
            {
                header.Clear();
                header.Write(cmd);
                header.Write(session);
            }
        }

        public void Unpack()
        {

        }
    }
}
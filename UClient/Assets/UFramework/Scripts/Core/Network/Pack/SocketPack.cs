/*
 * @Author: fasthro
 * @Date: 2020-11-09 10:44:57
 * @Description: socket pack
 */
using System.Text;
using LuaInterface;
using UFramework.Core;

namespace UFramework.Network
{
    /// <summary>
    /// 包类型
    /// </summary>
    public enum PackType
    {
        Binary, // byte[] [...]:data
        SizeBinary, // byte[] [0-1]:(ushort)size [...]:data
        SizeHeaderBinary,  // byte[] [0-1]:(ushort)size [2-SocketPackHeader.SIZE]:header [...]:data
    }

    /// <summary>
    /// 协议处理层
    /// </summary>
    public enum ProcessLayer
    {
        All,
        Lua,
        CSharp
    }


    public class SocketPack : IPoolBehaviour
    {
        /// <summary>
        /// 头长度
        /// byte[] [1-4]:cmd [5-8]:session [9-10]:layer
        /// </summary>
        public readonly static int HEADER_SIZE = 10;

        /// <summary>
        /// 小端字节序解析
        /// </summary>
        public readonly static bool LITTLE_ENDIAN = false;

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
        /// 协议处理层
        /// </summary>
        /// <value></value>
        public ProcessLayer layer { get; private set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        /// <value></value>
        public PackType packType { get; private set; }

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
        /// size bytes
        /// </summary>
        /// <value></value>
        public FixedByteArray sizer { get; private set; }

        /// <summary>
        /// header bytes
        /// </summary>
        /// <value></value>
        public FixedByteArray header { get; private set; }

        #region pool

        public bool isRecycled { get; set; }

        public static SocketPack AllocateReader(PackType packType, byte[] data)
        {
            return ObjectPool<SocketPack>.Instance.Allocate().InitializeReader(packType, null, data);
        }

        public static SocketPack AllocateReader(PackType packType, FixedByteArray header, byte[] data)
        {
            return ObjectPool<SocketPack>.Instance.Allocate().InitializeReader(packType, header, data);
        }

        public static SocketPack AllocateWriter(PackType packType, int cmd, ProcessLayer layer)
        {
            return ObjectPool<SocketPack>.Instance.Allocate().InitializeWriter(packType, cmd, layer);
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
            sizer = new FixedByteArray(2);
            header = new FixedByteArray(HEADER_SIZE);
        }

        private SocketPack InitializeReader(PackType packType, FixedByteArray fixHeader, byte[] data)
        {
            this.packType = packType;
            this.rawData = data;
            this.header.Clear();
            if (fixHeader != null)
            {
                this.header.Write(fixHeader.buffer);
                cmd = this.header.ReadInt32();
                session = this.header.ReadInt32();
                layer = (ProcessLayer)this.header.ReadInt16();
            }
            return this;
        }

        private SocketPack InitializeWriter(PackType packType, int cmd, ProcessLayer layer)
        {
            this.packType = packType;
            this.cmd = cmd;
            this.session = SESSION++;
            this.layer = layer;
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
            if (packType == PackType.SizeBinary || packType == PackType.SizeHeaderBinary)
            {
                sizer.Clear();
                switch (packType)
                {
                    case PackType.SizeBinary:
                        sizer.Write((ushort)rawDataSize);
                        break;
                    case PackType.SizeHeaderBinary:
                        sizer.Write((ushort)(SocketPack.HEADER_SIZE + rawDataSize));
                        break;
                }
            }

            if (packType == PackType.SizeHeaderBinary)
            {
                header.Clear();
                header.Write(cmd);
                header.Write(session);
                header.Write((short)layer);
            }
        }

        public void Unpack()
        {

        }
    }
}
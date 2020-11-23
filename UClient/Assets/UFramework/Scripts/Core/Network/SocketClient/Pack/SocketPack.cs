/*
 * @Author: fasthro
 * @Date: 2020-11-09 10:44:57
 * @Description: socket pack
 */
namespace UFramework.Network
{
    public enum SocketPackType
    {
        /// <summary>
        /// 行的形式读取和写入数据
        /// </summary>
        Line,

        /// <summary>
        /// Protobuf形式读取和写入数据
        /// </summary>
        Protobuf,
    }

    public abstract class SocketPack
    {
        /// <summary>
        /// 协议命令
        /// </summary>
        /// <value></value>
        public int CMD { get; protected set; }

        /// <summary>
        /// 协议唯一标识
        /// </summary>
        /// <value></value>
        public long session { get; protected set; }

        /// <summary>
        /// 头数据
        /// </summary>
        /// <value></value>
        public byte[] headData { get; protected set; }

        /// <summary>
        /// 数据
        /// </summary>
        /// <value></value>
        public byte[] rawData { get; protected set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        /// <value></value>
        public int rawDataSize { get { return rawData.Length; } }

        /// <summary>
        /// 包类型
        /// </summary>
        /// <value></value>
        public abstract SocketPackType packType { get; }

        /// <summary>
        /// 打包数据
        /// </summary>
        public virtual void Pack()
        {
            var head = new FixedByteArray(16);
            head.Write(rawData.Length + 16);
            headData = head.data;
        }

        public SocketPackLine ToLinePack() { return this as SocketPackLine; }
        public SocketPackProtobuf ToProtobufPack() { return this as SocketPackProtobuf; }
        public SocketPackStream ToStreamPack() { return this as SocketPackStream; }
    }
}
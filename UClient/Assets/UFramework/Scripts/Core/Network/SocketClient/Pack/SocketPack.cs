/*
 * @Author: fasthro
 * @Date: 2020-11-09 10:44:57
 * @Description: socket pack
 */
namespace UFramework.Network
{

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
        /// 打包数据
        /// </summary>
        public virtual void Pack()
        {
            var head = new FixedByteArray(16);
            head.Write(rawData.Length + 16);
            headData = head.data;
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
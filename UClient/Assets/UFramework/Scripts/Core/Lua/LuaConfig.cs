/*
 * @Author: fasthro
 * @Date: 2020-08-26 15:02:02
 * @Description: lua config
 */
using UFramework.Core;

namespace UFramework.Core
{
    public class LuaConfig : ISerializable
    {
        public SerializableAssigned assigned { get { return SerializableAssigned.Editor; } }

        /// <summary>
        /// 使用字节编码
        /// </summary>
        /// <value></value>
        public bool byteEncode { get; set; }

        /// <summary>
        /// lua search paths
        /// </summary>
        /// <value></value>
        public string[] searchPaths { get; set; }

        /// <summary>
        /// wrap class names
        /// </summary>
        /// <value></value>
        public string[] wrapClassNames { get; set; }

        public void Serialize()
        {
            Serializer<LuaConfig>.Serialize(this);
        }
    }
}

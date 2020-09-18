/*
 * @Author: fasthro
 * @Date: 2020-09-18 15:38:00
 * @Description: 磁盘文件
 */
using System.IO;

namespace UFramework.Asset
{
    public class DiskFile
    {
        public string hash { get; set; }

        public long id { get; set; }

        public long length { get; set; }

        public string name { get; set; }

        public long offset { get; set; }

        public DiskFile()
        {
            offset = -1;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(name);
            writer.Write(length);
            writer.Write(hash);
        }

        public void Deserialize(BinaryReader reader)
        {
            name = reader.ReadString();
            length = reader.ReadInt64();
            hash = reader.ReadString();
        }
    }
}
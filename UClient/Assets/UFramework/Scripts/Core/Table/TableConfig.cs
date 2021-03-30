/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:18:34
 * @Description: table serdata
 */

using System.Collections.Generic;
using UFramework.Core;

namespace UFramework.Core
{
    public enum TableKeyFormat
    {
        Default,
        IntKey,
        StringKey,
        Int2Key,
    }

    public class TableConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.AssetBundle;

        public bool isBinary = true;
        public Dictionary<string, TableKeyFormat> tableDict = new Dictionary<string, TableKeyFormat>();

        public void Serialize()
        {
            Serializer<TableConfig>.Serialize(this);
        }
    }
}
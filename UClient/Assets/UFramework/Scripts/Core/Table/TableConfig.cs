/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:18:34
 * @Description: table serdata
 */

using System.Collections.Generic;
using UFramework.Core;

namespace UFramework.Core
{
    public enum TableFormat
    {
        CSV,
        JSON,
        LUA,
    }

    public enum TableDataIndexFormat
    {
        Array,
        IntDictionary,
        StringDictionary,
        Int2IntDictionary,
    }

    public class TableConfig : ISerializable
    {
        public SerializableAssigned assigned { get { return SerializableAssigned.AssetBundle; } }

        public TableFormat outFormatOptions = TableFormat.CSV;
        public Dictionary<string, TableDataIndexFormat> tableDict = new Dictionary<string, TableDataIndexFormat>();

        public void Serialize()
        {
            Serializer<TableConfig>.Serialize(this);
        }
    }
}

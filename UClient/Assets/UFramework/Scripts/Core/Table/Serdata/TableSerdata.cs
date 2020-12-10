/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:18:34
 * @Description: table serdata
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Serialize;
using UnityEngine;

namespace UFramework.Table
{
    public class TableSerdata : ISerializable
    {
        public SerializableType serializableType { get { return SerializableType.AssetBundle; } }

        public FormatOptions outFormatOptions = FormatOptions.CSV;
        public Dictionary<string, DataFormatOptions> tableDict = new Dictionary<string, DataFormatOptions>();

        public void Serialization()
        {
            Serializable<TableSerdata>.Serialization(this);
        }
    }
}

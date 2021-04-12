// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-05-30 17:18:34
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UFramework.Core
{
    public enum TableKeyFormat
    {
        Default,
        IntKey,
        StringKey,
        Int2Key,
    }
    
    [System.Serializable]
    public class TableItem
    {
        [ShowInInspector, HideLabel, ReadOnly] [HorizontalGroup()]
        public string name;

        [ShowInInspector, HideLabel] [HorizontalGroup()]
        public TableKeyFormat format;

        [HideInInspector] public string dataMD5;
    }

    public class TableConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.AssetBundle;
        public Dictionary<string, TableItem> tableDict = new Dictionary<string, TableItem>();

        public void Serialize()
        {
            Serializer<TableConfig>.Serialize(this);
        }
    }
}
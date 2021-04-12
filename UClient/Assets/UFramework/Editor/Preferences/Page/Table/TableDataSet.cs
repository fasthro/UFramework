// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using UFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    [System.Serializable]
    public class TableEditorItem
    {
        public TableItem data;

        [HideInInspector] public string xmlMd5;
        [HideInInspector] public string xmlTempMD5;

        public string excelPath => IOPath.PathCombine(UApplication.AssetsDirectory, "Table/Excel", data.name + ".xlsx");
        public string structPath => IOPath.PathCombine("Assets/Scripts/Automatic/Table", data.name + "Table.cs");
        public string dataPath => IOPath.PathCombine(UApplication.AssetsDirectory, "Table/Data", data.name + ".bytes");
    }
    
    public class Preferences_Table_Config : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Editor;
        public string namespaceValue = "UFramework.Automatic";
        public Dictionary<string, TableEditorItem> tableDict = new Dictionary<string, TableEditorItem>();

        public void Serialize()
        {
            Serializer<Preferences_Table_Config>.Serialize(this);
        }
    }
}
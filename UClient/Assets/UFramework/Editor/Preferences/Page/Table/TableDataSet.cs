// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    [System.Serializable]
    public class TableItem
    {
        [ShowInInspector, HideLabel, ReadOnly] [HorizontalGroup("Table Name")]
        public string name;

        [ShowInInspector, HideLabel] [HorizontalGroup("Format")]
        public TableKeyFormat format;

        [HideInInspector] public string md5;

        public string excelPath => IOPath.PathCombine(UApplication.AssetsDirectory, "Table/Excel", name + ".xlsx");
        public string structPath => IOPath.PathCombine("Assets/Scripts/Automatic/Table", name + "Table.cs");
        public string dataPath => IOPath.PathCombine(UApplication.AssetsDirectory, "Table/Data", name + ".byte");
    }
}
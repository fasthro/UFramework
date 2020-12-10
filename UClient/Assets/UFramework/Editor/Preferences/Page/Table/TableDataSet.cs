/*
 * @Author: fasthro
 * @Date: 2020-12-10 14:51:32
 * @Description: 
 */
using UFramework.Table;
using Sirenix.OdinInspector;

namespace UFramework.Editor.Preferences
{
    [System.Serializable]
    public class TableItem
    {
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Table Name")]
        public string name;

        [ShowInInspector, HideLabel]
        [HorizontalGroup("Format")]
        public DataFormatOptions format;
    }
}
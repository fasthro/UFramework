/*
 * @Author: fasthro
 * @Date: 2020-06-29 11:26:04
 * @Description: Table Setting Page
 */
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UFramework.Config;
using UFramework.Table;

namespace UFramework.Editor.Preferences
{
    public class TablePreferencesPage : IPage
    {
        public string menuName { get { return "Table/Preferences"; } }

        /// <summary>
        /// 导出数据格式
        /// </summary>
        [InfoBox("数据表数据首选项")]
        public FormatOptions outFormatOptions = FormatOptions.CSV;

        /// <summary>
        /// 对象命名空间
        /// </summary>
        public string tableModelNamespace;

        static TableConfig describeObject;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<TableConfig>();
            outFormatOptions = describeObject.outFormatOptions;
            tableModelNamespace = describeObject.tableModelNamespace;
        }

        public void OnDrawFunctoinButton()
        {

        }

        public void OnApply()
        {
            describeObject.outFormatOptions = outFormatOptions;
            describeObject.tableModelNamespace = tableModelNamespace;
            describeObject.Save();
        }
    }
}
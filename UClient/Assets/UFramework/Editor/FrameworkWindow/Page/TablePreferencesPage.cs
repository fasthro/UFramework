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

namespace UFramework.FrameworkWindow
{
    public class TablePreferencesPage : IPage
    {
        public string menuName { get { return "Table/Preferences"; } }

        // 导出格式
        [InfoBox("数据表数据首选项")]
        public FormatOptions outFormatOptions = FormatOptions.CSV;

        // 命名空间
        public string tableModelNamespace;

        [Button(ButtonSizes.Large)]
        private void Apply()
        {
            var tableConfig = UConfig.Read<TableConfig>();
            tableConfig.outFormatOptions = outFormatOptions;
            tableConfig.tableModelNamespace = tableModelNamespace;
            tableConfig.Save();
        }

        public object GetInstance()
        {
            return this;
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }

        public void OnRenderBefore()
        {
            var tableConfig = UConfig.Read<TableConfig>();
            outFormatOptions = tableConfig.outFormatOptions;
            tableModelNamespace = tableConfig.tableModelNamespace;
        }
    }
}
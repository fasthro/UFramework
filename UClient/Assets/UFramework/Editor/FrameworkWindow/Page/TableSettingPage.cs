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
        static TablePreferencesPage instance;

        // 导出格式
        [InfoBox("数据表数据首选项")]
        public FormatOptions outFormatOptions = FormatOptions.CSV;

        // 命名空间
        public string tableModelNamespace;

        [Button(ButtonSizes.Large)]
        private void Apply()
        {
            var tableConfig = UConfig.Read<TableConfig>();
            tableConfig.outFormatOptions = instance.outFormatOptions;
            tableConfig.tableModelNamespace = instance.tableModelNamespace;
            tableConfig.Save();
        }

        public object GetInstance()
        {
            instance = new TablePreferencesPage();
            return instance;
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }

        public void OnRenderBefore()
        {
            var tableConfig = UConfig.Read<TableConfig>();
            instance.outFormatOptions = tableConfig.outFormatOptions;
            instance.tableModelNamespace = tableConfig.tableModelNamespace;
        }
    }
}
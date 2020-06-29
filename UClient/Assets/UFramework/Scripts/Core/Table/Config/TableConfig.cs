/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:18:34
 * @Description: table config
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Table
{
    public class TableConfig : IConfigObject
    {
        public string name { get { return "TableConfig"; } }

        // 导出格式
        [HideInInspector]
        public FormatOptions outFormatOptions = FormatOptions.CSV;

        // 命名空间
        [HideInInspector]
        public string tableModelNamespace;

        // 数据表
        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.OneLine)]
        [InfoBox("数据表数据存储格式设置")]
        public Dictionary<string, DataFormatOptions> tableDictionary = new Dictionary<string, DataFormatOptions>();

        /// <summary>
        /// Save
        /// </summary>
        [Button(ButtonSizes.Large, Name = "Apply")]
        [HorizontalGroup("Opt")]
        public void Save()
        {
            UConfig.Write<TableConfig>(this);
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        [ShowInInspector]
        [HorizontalGroup("Opt")]
        [Button(ButtonSizes.Large, Name = "Export")]
        static void Export()
        {
#if UNITY_EDITOR
            TableConfig tableConfig = UConfig.Read<TableConfig>();

            IOPath.DirectoryClear(App.TableObjectDirectory());
            IOPath.DirectoryClear(App.TableOutDataDirectory());

            tableConfig.tableDictionary.ForEach((item) =>
            {
                Debug.Log("Table Export: " + item.Key);
                var options = new ExcelReaderOptions();
                options.tableName = item.Key;
                options.tableModelNamespace = tableConfig.tableModelNamespace;
                options.outFormatOptions = tableConfig.outFormatOptions;
                options.dataFormatOptions = item.Value;
                options.dataOutDirectory = App.TableOutDataDirectory();
                options.tableModelOutDirectory = App.TableObjectDirectory();
                var reader = new ExcelReader(string.Format("{0}/{1}.xlsx", App.TableExcelDirectory(), item.Key), options);
                reader.Read();
                switch (tableConfig.outFormatOptions)
                {
                    case FormatOptions.CSV:
                        new Excel2CSV(reader);
                        break;
                    case FormatOptions.JSON:
                        new Excel2Json(reader);
                        break;
                    case FormatOptions.LUA:
                        new Excel2Lua(reader);
                        break;
                }
                new Excel2TableObject(reader);
            });

            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("Table Export Completed!");
#endif
        }
    }
}

/*
 * @Author: fasthro
 * @Date: 2020-06-29 11:26:04
 * @Description: Table Page
 */
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UFramework.Table;
using UnityEngine;

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


    public class TablePage : IPage, IPageBar
    {
        public string menuName { get { return "Table"; } }

        static TableConfig describeObject;

        static string rootPath;
        static string dataPath;
        static string excelPath;

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true, HideToolbar = true)]
        public List<TableItem> tables = new List<TableItem>();

        private Dictionary<string, DataFormatOptions> tableDictionary = new Dictionary<string, DataFormatOptions>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            rootPath = IOPath.PathCombine(App.AssetsDirectory, "Table");
            dataPath = IOPath.PathCombine(rootPath, "Data");
            excelPath = IOPath.PathCombine(rootPath, "Excel");

            describeObject = UConfig.Read<TableConfig>();

            bool hasNew = false;
            if (Directory.Exists(excelPath))
            {
                var files = Directory.GetFiles(excelPath, "*.xlsx", SearchOption.AllDirectories);
                HashSet<string> fileHashSet = new HashSet<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = IOPath.FileName(files[i], false);
                    fileHashSet.Add(fileName);
                    if (!describeObject.tableDictionary.ContainsKey(fileName))
                    {
                        hasNew = true;
                        describeObject.tableDictionary.Add(fileName, DataFormatOptions.Array);
                    }
                }

                List<string> removes = new List<string>();
                describeObject.tableDictionary.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                    {
                        removes.Add(item.Key);
                    }
                });

                for (int i = 0; i < removes.Count; i++)
                {
                    hasNew = true;
                    describeObject.tableDictionary.Remove(removes[i]);
                }
            }
            if (hasNew)
            {
                describeObject.Save();
            }

            tableDictionary = describeObject.tableDictionary;

            tables.Clear();
            foreach (KeyValuePair<string, DataFormatOptions> item in tableDictionary)
            {
                var tItem = new TableItem();
                tItem.name = item.Key;
                tItem.format = item.Value;

                tables.Add(tItem);
            }
        }

        public void OnPageBarDraw()
        {
            // 生成数据按钮
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate")))
            {
                TableConfig tableConfig = UConfig.Read<TableConfig>();

                var modePath = "Assets/Scripts/Automatic/Table";
                IOPath.DirectoryClear(modePath);
                IOPath.DirectoryClear(dataPath);

                tableConfig.tableDictionary.ForEach((System.Action<KeyValuePair<string, DataFormatOptions>>)((item) =>
                {
                    Logger.Debug("Table Export: " + item.Key);
                    var options = new ExcelReaderOptions();
                    options.tableName = item.Key;
                    options.outFormatOptions = tableConfig.outFormatOptions;
                    options.dataFormatOptions = item.Value;
                    options.dataOutDirectory = dataPath;
                    options.tableModelOutDirectory = modePath;
                    var reader = new ExcelReader(string.Format("{0}/{1}.xlsx", excelPath, item.Key), options);
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
                }));

                UnityEditor.AssetDatabase.Refresh();
                Logger.Debug("Table Export Completed!");
            }
        }

        public void OnSaveDescribe()
        {
            if (describeObject == null) return;

            foreach (var item in tables)
            {
                tableDictionary[item.name] = item.format;
            }
            describeObject.tableDictionary = tableDictionary;
            describeObject.Save();
        }
    }
}
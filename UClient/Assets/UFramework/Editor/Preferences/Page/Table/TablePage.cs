/*
 * @Author: fasthro
 * @Date: 2020-06-29 11:26:04
 * @Description: Table Page
 */
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Serialize;
using UFramework.Table;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class TablePage : IPage, IPageBar
    {
        public string menuName { get { return "Table"; } }

        static string RootPath;
        static string DataPath;
        static string ExcelPath;

        static TableSerdata Serdata { get { return Serializable<TableSerdata>.Instance; } }

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true, HideToolbar = true)]
        public List<TableItem> tables = new List<TableItem>();

        private Dictionary<string, DataFormatOptions> _tableDict = new Dictionary<string, DataFormatOptions>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            RootPath = IOPath.PathCombine(App.AssetsDirectory, "Table");
            DataPath = IOPath.PathCombine(RootPath, "Data");
            ExcelPath = IOPath.PathCombine(RootPath, "Excel");

            bool hasNew = false;
            if (Directory.Exists(ExcelPath))
            {
                var files = Directory.GetFiles(ExcelPath, "*.xlsx", SearchOption.AllDirectories);
                HashSet<string> fileHashSet = new HashSet<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = IOPath.FileName(files[i], false);
                    fileHashSet.Add(fileName);
                    if (!Serdata.tableDict.ContainsKey(fileName))
                    {
                        hasNew = true;
                        Serdata.tableDict.Add(fileName, DataFormatOptions.Array);
                    }
                }

                List<string> removes = new List<string>();
                Serdata.tableDict.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                    {
                        removes.Add(item.Key);
                    }
                });

                for (int i = 0; i < removes.Count; i++)
                {
                    hasNew = true;
                    Serdata.tableDict.Remove(removes[i]);
                }
            }
            if (hasNew)
            {
                Serdata.Serialization();
            }

            _tableDict = Serdata.tableDict;

            tables.Clear();
            foreach (KeyValuePair<string, DataFormatOptions> item in _tableDict)
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
                var modePath = "Assets/Scripts/Automatic/Table";
                IOPath.DirectoryClear(modePath);
                IOPath.DirectoryClear(DataPath);

                Serdata.tableDict.ForEach((System.Action<KeyValuePair<string, DataFormatOptions>>)((item) =>
                {
                    Logger.Debug("Table Export: " + item.Key);
                    var options = new ExcelReaderOptions();
                    options.tableName = item.Key;
                    options.outFormatOptions = Serdata.outFormatOptions;
                    options.dataFormatOptions = item.Value;
                    options.dataOutDirectory = DataPath;
                    options.tableModelOutDirectory = modePath;
                    var reader = new ExcelReader(string.Format("{0}/{1}.xlsx", ExcelPath, item.Key), options);
                    reader.Read();
                    switch (Serdata.outFormatOptions)
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
            foreach (var item in tables)
            {
                _tableDict[item.name] = item.format;
            }
            Serdata.tableDict = _tableDict;
            Serdata.Serialization();
        }
    }
}
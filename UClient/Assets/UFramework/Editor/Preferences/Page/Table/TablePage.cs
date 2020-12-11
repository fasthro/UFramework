/*
 * @Author: fasthro
 * @Date: 2020-06-29 11:26:04
 * @Description: Table Page
 */
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class TablePage : IPage, IPageBar
    {
        public string menuName { get { return "Table"; } }

        static string RootPath;
        static string DataPath;
        static string ExcelPath;

        static TableConfig Config { get { return Serializer<TableConfig>.Instance; } }

        [ShowInInspector]
        [TableList(IsReadOnly = true, AlwaysExpanded = true, HideToolbar = true)]
        public List<TableItem> tables = new List<TableItem>();

        private Dictionary<string, TableDataIndexFormat> _tableDict = new Dictionary<string, TableDataIndexFormat>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            RootPath = IOPath.PathCombine(UApplication.AssetsDirectory, "Table");
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
                    if (!Config.tableDict.ContainsKey(fileName))
                    {
                        hasNew = true;
                        Config.tableDict.Add(fileName, TableDataIndexFormat.Array);
                    }
                }

                List<string> removes = new List<string>();
                Config.tableDict.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                    {
                        removes.Add(item.Key);
                    }
                });

                for (int i = 0; i < removes.Count; i++)
                {
                    hasNew = true;
                    Config.tableDict.Remove(removes[i]);
                }
            }
            if (hasNew)
            {
                Config.Serialize();
            }

            _tableDict = Config.tableDict;

            tables.Clear();
            foreach (KeyValuePair<string, TableDataIndexFormat> item in _tableDict)
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

                Config.tableDict.ForEach((System.Action<KeyValuePair<string, TableDataIndexFormat>>)((item) =>
                {
                    Logger.Debug("Table Export: " + item.Key);
                    var options = new ExcelReaderOptions();
                    options.tableName = item.Key;
                    options.outFormatOptions = Config.outFormatOptions;
                    options.dataFormatOptions = item.Value;
                    options.dataOutDirectory = DataPath;
                    options.tableModelOutDirectory = modePath;
                    var reader = new ExcelReader(string.Format("{0}/{1}.xlsx", ExcelPath, item.Key), options);
                    reader.Read();
                    switch (Config.outFormatOptions)
                    {
                        case TableFormat.CSV:
                            new Excel2CSV(reader);
                            break;
                        case TableFormat.JSON:
                            new Excel2Json(reader);
                            break;
                        case TableFormat.LUA:
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
            Config.tableDict = _tableDict;
            Config.Serialize();
        }
    }
}
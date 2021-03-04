// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

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
        public string menuName => "Table";

        static string RootPath;
        static string DataPath;
        static string ExcelPath;

        static TableConfig Config => Serializer<TableConfig>.Instance;

        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true, HideToolbar = true)]
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

            var hasNew = false;
            if (Directory.Exists(ExcelPath))
            {
                var files = Directory.GetFiles(ExcelPath, "*.xlsx", SearchOption.AllDirectories);
                var fileHashSet = new HashSet<string>();
                for (var i = 0; i < files.Length; i++)
                {
                    var fileName = IOPath.FileName(files[i], false);
                    fileHashSet.Add(fileName);
                    if (!Config.tableDict.ContainsKey(fileName))
                    {
                        hasNew = true;
                        Config.tableDict.Add(fileName, TableDataIndexFormat.Array);
                    }
                }

                var removes = new List<string>();
                Config.tableDict.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                    {
                        removes.Add(item.Key);
                    }
                });

                for (var i = 0; i < removes.Count; i++)
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
            foreach (var item in _tableDict)
            {
                tables.Add(new TableItem {name = item.Key, format = item.Value});
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

                Config.tableDict.ForEach((System.Action<KeyValuePair<string, TableDataIndexFormat>>) ((item) =>
                {
                    Logger.Debug("Table Export: " + item.Key);
                    var options = new ExcelReaderOptions
                    {
                        tableName = item.Key,
                        outFormatOptions = Config.outFormatOptions,
                        dataFormatOptions = item.Value,
                        dataOutDirectory = DataPath,
                        tableModelOutDirectory = modePath
                    };
                    var reader = new ExcelReader($"{ExcelPath}/{item.Key}.xlsx", options);
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
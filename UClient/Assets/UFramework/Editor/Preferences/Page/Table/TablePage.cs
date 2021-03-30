// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Entitas.CodeGeneration.Plugins;
using OfficeOpenXml;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class TablePage : IPage, IPageBar
    {
        public string menuName => "Table";

        static TableConfig Config => Serializer<TableConfig>.Instance;

        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true, HideToolbar = true)]
        public List<TableItem> tables = new List<TableItem>();

        private Dictionary<string, TableKeyFormat> _tableDict = new Dictionary<string, TableKeyFormat>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            RefreshTableList();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate All")))
            {
                foreach (var table in tables)
                {
                    var newMd5 = IOPath.FileMD5(table.excelPath);
                    ProcTable(table, newMd5);
                }
                OnSaveDescribe();
                UnityEditor.AssetDatabase.Refresh();
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate Modify")))
            {
                foreach (var table in tables)
                {
                    var newMd5 = IOPath.FileMD5(table.excelPath);
                    if (table.md5 == null || !table.md5.Equals(newMd5))
                        ProcTable(table, newMd5);
                }
                OnSaveDescribe();
                UnityEditor.AssetDatabase.Refresh();
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

        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshTableList()
        {
            var excelDir = IOPath.PathCombine(UApplication.AssetsDirectory, "Table/Excel");
            var hasNew = false;
            if (Directory.Exists(excelDir))
            {
                var files = Directory.GetFiles(excelDir, "*.xlsx", SearchOption.AllDirectories);
                var fileHashSet = new HashSet<string>();
                for (var i = 0; i < files.Length; i++)
                {
                    var fileName = IOPath.FileName(files[i], false);
                    fileHashSet.Add(fileName);
                    if (!Config.tableDict.ContainsKey(fileName))
                    {
                        hasNew = true;
                        Config.tableDict.Add(fileName, TableKeyFormat.Default);
                    }
                }

                var removes = new List<string>();
                Config.tableDict.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                        removes.Add(item.Key);
                });

                if (removes.Count > 0)
                    hasNew = true;

                for (var i = 0; i < removes.Count; i++)
                    Config.tableDict.Remove(removes[i]);
            }

            if (hasNew)
                Config.Serialize();

            _tableDict = Config.tableDict;

            tables.Clear();
            foreach (var item in _tableDict)
                tables.Add(new TableItem {name = item.Key, format = item.Value});
        }

        #region proc table

        private void ProcTable(TableItem item, string newMD5)
        {
            using (var fs = new FileStream(item.excelPath, FileMode.Open))
            {
                using (var package = new ExcelPackage(fs))
                {
                    ExcelWorksheet sheet = null;
                    var count = package.Workbook.Worksheets.Count;
                    for (var i = 1; i <= count; ++i)
                    {
                        sheet = package.Workbook.Worksheets[i];
                        if (!sheet.Cells.Any())
                            continue;
                        // TODO
                    }

                    if (count > 0)
                        GenerateTableStruct(item, sheet);

                    item.md5 = newMD5;
                }
            }
        }

        #endregion

        #region gen c#

        private void GenerateTableStruct(TableItem item, ExcelWorksheet sheet)
        {
            var colNum = sheet.Dimension.End.Column;

            var varBody = new StringBuilder();

            for (var i = 1; i <= colNum; i++)
            {
                var desc = sheet.GetValue(1, i);
                var field = sheet.GetValue(2, i);
                var t = sheet.GetValue(3, i);
                if (t == null || field == null)
                {
                    if (t != null)
                    {
                        if (IgnoreType(t.ToString()))
                            continue;
                    }

                    Logger.Error($"{item.name} table excel data error!");
                    return;
                }

                if (IgnoreType(t.ToString()))
                    continue;

                varBody.AppendLine("        /// <summary>");
                varBody.AppendLine($"        /// {(desc != null ? desc.ToString() : string.Empty)}");
                varBody.AppendLine("        /// <summary>");

                var ts = t.ToString();

                if (ts.Equals("lang"))
                {
                    ts = "LocalizationText";
                }
                else if (ts.Equals("langs"))
                {
                    ts = "LocalizationText[]";
                }

                varBody.AppendLine($"        public {ts} {field};");
                varBody.AppendLine("");
            }

            Debug.Log(varBody.ToString());

            var template = IOPath.FileReadText(IOPath.PathCombine(UApplication.AssetsDirectory, "Table/TableStruct.txt"));
            template = template.Replace("$tableName$", item.name);
            template = template.Replace("$variable$", varBody.ToString());
            template = template.Replace("$keyFormat$", _tableDict[item.name].ToString());


            IOPath.FileCreateText(item.structPath, template);
        }

        #endregion

        #region gen byte
        

        #endregion

        #region gen lua table

        
        #endregion
        
        #region common

        static bool IgnoreType(string t)
        {
            return t.Equals("ignore");
        }

        #endregion
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using OfficeOpenXml;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class TablePage : IPage, IPageBar, IPageCallback
    {
        public string menuName => "Table";

        static TableConfig Config => Serializer<TableConfig>.Instance;
        static Preferences_Table_Config EditorConfig => Serializer<Preferences_Table_Config>.Instance;

        public bool useNamespace;
        private bool _useNamespace => !useNamespace;

        [HideIf("_useNamespace")] public string namespaceValue;

        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true, HideToolbar = true)]
        public List<TableEditorItem> tables = new List<TableEditorItem>();

        private Dictionary<string, TableKeyFormat> _tableDict = new Dictionary<string, TableKeyFormat>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            useNamespace = !string.IsNullOrEmpty(EditorConfig.namespaceValue);
            namespaceValue = EditorConfig.namespaceValue;
            RefreshTableList();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate All")))
            {
                foreach (var table in tables)
                {
                    var newMd5 = IOPath.FileMD5(table.excelPath);
                    ProcTable(table, newMd5, false);
                }

                OnSaveDescribe();
                AssetDatabase.Refresh();
                PreferencesWindow.RsegisterCallbackWithEditorCompile(this);
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate Modify")))
            {
                bool have = false;
                foreach (var table in tables)
                {
                    var newMd5 = IOPath.FileMD5(table.excelPath);
                    if (table.xmlMd5 == null || !table.xmlMd5.Equals(newMd5))
                    {
                        ProcTable(table, newMd5, false);
                        have = true;
                    }
                }

                OnSaveDescribe();
                AssetDatabase.Refresh();

                if (have)
                    PreferencesWindow.RsegisterCallbackWithEditorCompile(this);
            }
        }

        public void OnSaveDescribe()
        {
            EditorConfig.tableDict.Clear();
            Config.tableDict.Clear();
            foreach (var item in tables)
            {
                EditorConfig.tableDict.Add(item.data.name, item);
                Config.tableDict.Add(item.data.name, item.data);
            }

            EditorConfig.namespaceValue = useNamespace ? namespaceValue : string.Empty;

            EditorConfig.Serialize();
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
                    if (!EditorConfig.tableDict.ContainsKey(fileName))
                    {
                        hasNew = true;
                        EditorConfig.tableDict.Add(fileName, new TableEditorItem()
                        {
                            data = new TableItem()
                            {
                                name = fileName,
                                dataMD5 = string.Empty,
                                format = TableKeyFormat.Default
                            },
                            xmlMd5 = string.Empty,
                            xmlTempMD5 = string.Empty
                        });
                    }
                }

                var removes = new List<string>();
                EditorConfig.tableDict.ForEach((item) =>
                {
                    if (!fileHashSet.Contains(item.Key))
                        removes.Add(item.Key);
                });

                if (removes.Count > 0)
                    hasNew = true;

                for (var i = 0; i < removes.Count; i++)
                    EditorConfig.tableDict.Remove(removes[i]);
            }

            if (hasNew)
                EditorConfig.Serialize();

            tables.Clear();
            foreach (var item in EditorConfig.tableDict)
                tables.Add(item.Value);
        }

        #region gen c#

        private void ProcTable(TableEditorItem item, string newMD5, bool bytes)
        {
            using (var fs = new FileStream(item.excelPath, FileMode.Open))
            {
                using (var package = new ExcelPackage(fs))
                {
                    var count = package.Workbook.Worksheets.Count;
                    if (count > 0)
                    {
                        var sheet = package.Workbook.Worksheets[1];
                        if (!bytes)
                        {
                            GenerateTableStruct(item, sheet);
                            item.xmlMd5 = string.Empty;
                            item.xmlTempMD5 = newMD5;
                        }
                        else
                        {
                            GenerateBytes(item, sheet);
                            item.data.dataMD5 = IOPath.FileMD5(item.dataPath);
                            item.xmlMd5 = item.xmlTempMD5;
                        }
                    }
                }
            }
        }

        private void GenerateTableStruct(TableEditorItem item, ExcelWorksheet sheet)
        {
            var colNum = sheet.Dimension.End.Column;

            var varBody = new StringBuilder();
            var keyField1 = string.Empty;
            var keyField2 = string.Empty;

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

                    Logger.Error($"{item.data.name} table excel data error!");
                    return;
                }

                if (IgnoreType(t.ToString()))
                    continue;

                varBody.AppendLine("        /// <summary>");
                varBody.AppendLine($"        /// {(desc != null ? desc.ToString() : string.Empty)}");
                varBody.AppendLine("        /// <summary>");

                varBody.AppendLine($"        public {t.ToString()} {field};");
                varBody.AppendLine("");

                if (i == 1) keyField1 = field.ToString();
                if (i == 2) keyField2 = field.ToString();
            }

            var template =
                IOPath.FileReadText(IOPath.PathCombine(UApplication.AssetsDirectory,
                    useNamespace ? "Table/Template/TableStructNamespace.txt" : "Table/Template/TableStruct.txt"));
            template = template.Replace("$Date$", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            template = template.Replace("$tableName$", item.data.name);
            template = template.Replace("$variable$", varBody.ToString());
            template = template.Replace("$keyFormat$", item.data.format.ToString());

            if (!string.IsNullOrEmpty(keyField1))
                template = template.Replace("$key1$", keyField1);

            if (!string.IsNullOrEmpty(keyField2))
                template = template.Replace("$key2$", keyField2);

            if (useNamespace)
                template = template.Replace("$namespace$", namespaceValue);

            IOPath.FileCreateText(item.structPath, template);
        }

        #endregion

        #region gen byte

        private void GenerateBytes(TableEditorItem item, ExcelWorksheet sheet)
        {
            var datasTypeName = useNamespace
                ? $"{namespaceValue}.{item.data.name}TableDatas"
                : $"{item.data.name}TableDatas";
            var dataTypeName =
                useNamespace ? $"{namespaceValue}.{item.data.name}TableData" : $"{item.data.name}TableData";
            var assembly = Assembly.Load("Assembly-CSharp");
            var instance = assembly.CreateInstance(datasTypeName);
            if (instance == null)
            {
                Logger.Error($"table generate {item.data.name} bytes failed!");
                return;
            }

            var datasType = assembly.GetType(datasTypeName);
            var methodInfo = datasType.GetMethod("AddData", BindingFlags.Instance | BindingFlags.Public);
            var dataType = assembly.GetType(dataTypeName);

            var colNum = sheet.Dimension.End.Column;
            var rowNum = sheet.Dimension.End.Row;

            var vs = new List<string>();
            var ts = new List<string>();
            for (var i = 1; i <= colNum; i++)
            {
                var varName = sheet.GetValue(2, i) as string;
                var varType = sheet.GetValue(3, i).ToString();
                if (string.IsNullOrEmpty(varName) || IgnoreType(varType))
                    continue;
                vs.Add(varName);
                ts.Add(varType);
            }

            var splitChar = ',';
            var splitChar2 = '|';
            for (var i = 4; i <= rowNum; i++)
            {
                var dataInstance = Activator.CreateInstance(dataType);
                for (var j = 1; j <= vs.Count; j++)
                {
                    var varName = vs[j - 1];
                    var varType = ts[j - 1];
                    var value = sheet.GetValue(i, j);
                    if (value == null)
                        continue;

                    var valueStr = value.ToString();
                    object valueObject = null;
                    switch (varType)
                    {
                        case "byte":
                            valueObject = byte.Parse(valueStr);
                            break;
                        case "int":
                            valueObject = int.Parse(valueStr);
                            break;
                        case "long":
                            valueObject = long.Parse(valueStr);
                            break;
                        case "float":
                            valueObject = float.Parse(valueStr);
                            break;
                        case "double":
                            valueObject = double.Parse(valueStr);
                            break;
                        case "bool":
                            valueObject = ToBool(valueStr);
                            break;
                        case "string":
                            valueObject = valueStr;
                            break;
                        case "Vector2":
                            valueObject = ToVector2(valueStr, splitChar);
                            break;
                        case "Vector3":
                            valueObject = ToVector3(valueStr, splitChar);
                            break;
                        case "Color":
                            valueObject = ToColor(valueStr, splitChar);
                            break;
                        case "Color32":
                            valueObject = ToColor32(valueStr, splitChar);
                            break;
                        case "byte[]":
                            valueObject = ToByteArray(valueStr, splitChar);
                            break;
                        case "int[]":
                            valueObject = ToIntArray(valueStr, splitChar);
                            break;
                        case "long[]":
                            valueObject = ToLongArray(valueStr, splitChar);
                            break;
                        case "float[]":
                            valueObject = ToFloatArray(valueStr, splitChar);
                            break;
                        case "double[]":
                            valueObject = ToDoubleArray(valueStr, splitChar);
                            break;
                        case "bool[]":
                            valueObject = ToBoolArray(valueStr, splitChar);
                            break;
                        case "string[]":
                            valueObject = ToStringArray(valueStr, splitChar);
                            break;
                        case "Vector2[]":
                            valueObject = ToVector2Array(valueStr, splitChar, splitChar2);
                            break;
                        case "Vector3[]":
                            valueObject = ToVector3Array(valueStr, splitChar, splitChar2);
                            break;
                        case "Color[]":
                            valueObject = ToColorArray(valueStr, splitChar, splitChar2);
                            break;
                        case "Color32[]":
                            valueObject = ToColor32Array(valueStr, splitChar, splitChar2);
                            break;
                    }

//,,,,,,Vector2[],Vector3[],langs
                    dataInstance.GetType().GetField(varName).SetValue(dataInstance, valueObject);
                }

                methodInfo?.Invoke(instance, new object[] {dataInstance});
            }

            IOPath.FileDelete(item.dataPath);
            TableSerialize.Serialize(item.dataPath, instance);
        }

        #endregion

        #region gen lua table

        #endregion

        #region common

        static bool IgnoreType(string t)
        {
            return t.Trim().Equals("ignore");
        }

        #endregion

        #region utils

        static bool ToBool(string input)
        {
            return !string.IsNullOrEmpty(input) && !input.Equals("0");
        }

        static Vector2 ToVector2(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return new Vector2(0, 0);
            var res = input.Split(splitChar);
            return res.Length == 1
                ? new Vector2(float.Parse(res[0]), 0)
                : new Vector2(float.Parse(res[0]), float.Parse(res[1]));
        }

        static Vector3 ToVector3(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return new Vector3(0, 0, 0);
            var res = input.Split(splitChar);
            if (res.Length == 1)
                return new Vector3(float.Parse(res[0]), 0, 0);
            else if (res.Length == 2)
                return new Vector3(float.Parse(res[0]), float.Parse(res[1]), 0);
            else return new Vector3(float.Parse(res[0]), float.Parse(res[1]), float.Parse(res[2]));
        }

        static Color ToColor(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return new Color(0, 0, 0, 0);
            var res = input.Split(splitChar);
            return new Color(float.Parse(res[0]), float.Parse(res[1]), float.Parse(res[2]), float.Parse(res[3]));
        }

        static Color32 ToColor32(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return new Color32(0, 0, 0, 0);
            var res = input.Split(splitChar);
            return new Color32(byte.Parse(res[0]), byte.Parse(res[1]), byte.Parse(res[2]), byte.Parse(res[3]));
        }

        static byte[] ToByteArray(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitChar);
            var array = new byte[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = byte.Parse(res[i]);
            return array;
        }

        static int[] ToIntArray(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitChar);
            var array = new int[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = int.Parse(res[i]);
            return array;
        }

        static float[] ToFloatArray(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitChar);
            var array = new float[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = float.Parse(res[i]);
            return array;
        }

        static long[] ToLongArray(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitChar);
            var array = new long[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = long.Parse(res[i]);
            return array;
        }

        static double[] ToDoubleArray(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitChar);
            var array = new double[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = double.Parse(res[i]);
            return array;
        }

        static bool[] ToBoolArray(string input, char splitChar)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitChar);
            var array = new bool[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = bool.Parse(res[i]);
            return array;
        }

        static Vector2[] ToVector2Array(string input, char splitChar, char splitCharArray)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitCharArray);
            var array = new Vector2[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = ToVector2(res[i], splitChar);
            return array;
        }

        static Vector3[] ToVector3Array(string input, char splitChar, char splitCharArray)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitCharArray);
            var array = new Vector3[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = ToVector3(res[i], splitChar);
            return array;
        }

        static Color[] ToColorArray(string input, char splitChar, char splitCharArray)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitCharArray);
            var array = new Color[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = ToColor(res[i], splitChar);
            return array;
        }

        static Color32[] ToColor32Array(string input, char splitChar, char splitCharArray)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var res = input.Split(splitCharArray);
            var array = new Color32[res.Length];
            for (var i = 0; i < res.Length; i++)
                array[i] = ToColor(res[i], splitChar);
            return array;
        }

        static string[] ToStringArray(string input, char splitChar)
        {
            return string.IsNullOrEmpty(input) ? null : input.Split(splitChar);
        }

        #endregion

        public void OnEdittorCompileCallback()
        {
            foreach (var item in tables)
                if (!item.xmlMd5.Equals(item.xmlTempMD5))
                    ProcTable(item, item.xmlTempMD5, true);

            OnSaveDescribe();
            AssetDatabase.Refresh();
        }
    }
}
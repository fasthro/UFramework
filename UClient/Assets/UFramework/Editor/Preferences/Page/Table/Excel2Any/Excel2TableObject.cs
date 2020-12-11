/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 csharp object
 */
using System.Collections.Generic;
using System.Text;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class Excel2TableObject : Excel2Any
    {
        private string Template = @"// UFramework Automatic.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFramework.Core;

namespace UFramework.Automatic
{
    public class $tableName$TableData
    {
$variable$
    }

    public class $tableName$Table : Singleton<$tableName$Table>, ITableBehaviour
    {
        public string tableName { get { return $tableNameStr$; } }
        public int maxCount { get { return m_tableDatas.Length; } }
        
        public TableDataIndexFormat dataFormatOptions = TableDataIndexFormat.$dataFormat$;
        private $tableName$TableData[] m_tableDatas;
        private Dictionary<int, $tableName$TableData> m_tableDataIntDictionary;
        private Dictionary<string, $tableName$TableData> m_tableDataStringDictionary;
        private Dictionary<int, Dictionary<int, $tableName$TableData>> m_tableDataInt2IntDictionary;

        protected override void OnSingletonAwake()
        {
            switch (dataFormatOptions)
            {
                case TableDataIndexFormat.Array:
                    m_tableDatas = new TableParseCSV(tableName).ParseArray<$tableName$TableData>();
                    break;
                case TableDataIndexFormat.IntDictionary:
                    m_tableDataIntDictionary = new TableParseCSV(tableName).ParseIntDictionary<$tableName$TableData>();
                    break;
                case TableDataIndexFormat.StringDictionary:
                    m_tableDataStringDictionary = new TableParseCSV(tableName).ParseStringDictionary<$tableName$TableData>();
                    break;
                case TableDataIndexFormat.Int2IntDictionary:
                    m_tableDataInt2IntDictionary = new TableParseCSV(tableName).ParseInt2IntDictionary<$tableName$TableData>();
                    break;
            }
        }

        private $tableName$TableData _GetIndexData(int index)
        {
            if (dataFormatOptions == TableDataIndexFormat.Array)
            {
                if (index >= 0 && index < m_tableDatas.Length)
                {
                    return m_tableDatas[index];
                }
            }
            else $GetIndexDataError$
            return null;
        }

        private $tableName$TableData _GetKeyData(int key)
        {
            if (dataFormatOptions == TableDataIndexFormat.IntDictionary)
            {
                $tableName$TableData data = null;
                if (m_tableDataIntDictionary.TryGetValue(key, out data))
                {
                    return data;
                }
            }
            else $GetIntKeyDataError$
            return null;
        }

        private $tableName$TableData _GetKeyData(string key)
        {
            if (dataFormatOptions == TableDataIndexFormat.StringDictionary)
            {
                $tableName$TableData data = null;
                if (m_tableDataStringDictionary.TryGetValue(key, out data))
                {
                    return data;
                }
            }
            else $GetStringKeyDataError$
            return null;
        }

        private $tableName$TableData _GetKeyData(int key1, int key2)
        {
            if (dataFormatOptions == TableDataIndexFormat.Int2IntDictionary)
            {
                Dictionary<int, $tableName$TableData> dictionary = null;
                if (m_tableDataInt2IntDictionary.TryGetValue(key1, out dictionary))
                {
                    $tableName$TableData data = null;
                    if (dictionary.TryGetValue(key2, out data))
                    {
                        return data;
                    }
                }
            }
            else $GetInt2IntKeyDataError$
            return null;
        }

        public static $tableName$TableData GetIndexData(int index) { return Instance._GetIndexData(index); }
        public static $tableName$TableData GetKeyData(int key) { return Instance._GetKeyData(key); }
        public static $tableName$TableData GetKeyData(string key) { return Instance._GetKeyData(key); }
        public static $tableName$TableData GetKeyData(int key1, int key2) { return Instance._GetKeyData(key1, key2); }
    }
}";

        private StringBuilder _sb = new StringBuilder();

        public Excel2TableObject(ExcelReader reader) : base(reader)
        {
            Template = Template.Replace("$tableName$", reader.options.tableName);
            Template = Template.Replace("$tableNameStr$", string.Format("\"{0}\"", reader.options.tableName));
            Template = Template.Replace("$dataFormat$", reader.options.dataFormatOptions.ToString());
            Template = Template.Replace("$GetIndexDataError$", string.Format("Debug.LogError(\"[{0}Table] TableDataIndexFormat: {1}. Please use the GetKeyData(index)\");", reader.options.tableName, TableDataIndexFormat.Array.ToString()));
            Template = Template.Replace("$GetIntKeyDataError$", string.Format("Debug.LogError(\"[{0}Table] TableDataIndexFormat: {1}. Please use the GetKeyData(int-key)\");", reader.options.tableName, TableDataIndexFormat.IntDictionary.ToString()));
            Template = Template.Replace("$GetStringKeyDataError$", string.Format("Debug.LogError(\"[{0}Table] TableDataIndexFormat: {1}. Please use the GetKeyData(string-key)\");", reader.options.tableName, TableDataIndexFormat.StringDictionary.ToString()));
            Template = Template.Replace("$GetInt2IntKeyDataError$", string.Format("Debug.LogError(\"[{0}Table] TableDataIndexFormat: {1}. Please use the GetKeyData(int-key, int-key)\");", reader.options.tableName, TableDataIndexFormat.Int2IntDictionary.ToString()));

            for (int i = 0; i < reader.fields.Count; i++)
            {
                _sb.AppendLine("        // " + reader.descriptions[i]);
                _sb.AppendLine(string.Format("        public {0} {1};", TableTypeUtils.FieldTypeToTypeContent(reader.types[i]), reader.fields[i]));
                // 方便国家化字段调用,为国际化添加字段
                if (reader.types[i] == TableFieldType.Language)
                {
                    _sb.AppendLine(string.Format("        public string {0}_language {{ get {{ return {1} != null ? {2}.ToString() : string.Empty; }} }}", reader.fields[i], reader.fields[i], reader.fields[i]));
                }
            }
            Template = Template.Replace("$variable$", _sb.ToString());

            IOPath.FileCreateText(reader.options.tableModelOutFilePath, Template);
        }
    }
}
// UFramework Automatic.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFramework.Core;

namespace UFramework.Automatic
{
    public class TemplateTableData
    {
        // 键值
        public int key;
        // 描述1
        public byte Byte;
        // 描述2
        public int Int;
        // 描述3
        public long Long;
        // 描述4
        public float Float;
        // 描述5
        public double Double;
        // 描述6
        public string String;
        // 描述7
        public bool Boolean;
        // 描述8
        public Vector2 Vector2;
        // 描述9
        public Vector3 Vector3;
        // 多语言
        public LocalizationText language;
        public string language_language { get { return language != null ? language.ToString() : string.Empty; } }
        // 描述10
        public byte[] ArrayByte;
        // 描述11
        public int[] ArrayInt;
        // 描述12
        public long[] ArrayLong;
        // 描述13
        public float[] ArrayFloat;
        // 描述14
        public double[] ArrayDouble;
        // 描述15
        public string[] ArrayString;
        // 描述16
        public bool[] ArrayBoolean;
        // 描述17
        public Vector2[] ArrayVector2;
        // 描述18
        public Vector3[] ArrayVector3;
        // 多语言数组
        public LocalizationText[] languages;

    }

    public class TemplateTable : Singleton<TemplateTable>, ITableBehaviour
    {
        public string tableName { get { return "Template"; } }
        public int maxCount { get { return m_tableDatas.Length; } }
        
        public TableDataIndexFormat dataFormatOptions = TableDataIndexFormat.Array;
        private TemplateTableData[] m_tableDatas;
        private Dictionary<int, TemplateTableData> m_tableDataIntDictionary;
        private Dictionary<string, TemplateTableData> m_tableDataStringDictionary;
        private Dictionary<int, Dictionary<int, TemplateTableData>> m_tableDataInt2IntDictionary;

        protected override void OnSingletonAwake()
        {
            switch (dataFormatOptions)
            {
                case TableDataIndexFormat.Array:
                    m_tableDatas = new TableParseCSV(tableName).ParseArray<TemplateTableData>();
                    break;
                case TableDataIndexFormat.IntDictionary:
                    m_tableDataIntDictionary = new TableParseCSV(tableName).ParseIntDictionary<TemplateTableData>();
                    break;
                case TableDataIndexFormat.StringDictionary:
                    m_tableDataStringDictionary = new TableParseCSV(tableName).ParseStringDictionary<TemplateTableData>();
                    break;
                case TableDataIndexFormat.Int2IntDictionary:
                    m_tableDataInt2IntDictionary = new TableParseCSV(tableName).ParseInt2IntDictionary<TemplateTableData>();
                    break;
            }
        }

        private TemplateTableData _GetIndexData(int index)
        {
            if (dataFormatOptions == TableDataIndexFormat.Array)
            {
                if (index >= 0 && index < m_tableDatas.Length)
                {
                    return m_tableDatas[index];
                }
            }
            else Debug.LogError("[TemplateTable] TableDataIndexFormat: Array. Please use the GetKeyData(index)");
            return null;
        }

        private TemplateTableData _GetKeyData(int key)
        {
            if (dataFormatOptions == TableDataIndexFormat.IntDictionary)
            {
                TemplateTableData data = null;
                if (m_tableDataIntDictionary.TryGetValue(key, out data))
                {
                    return data;
                }
            }
            else Debug.LogError("[TemplateTable] TableDataIndexFormat: IntDictionary. Please use the GetKeyData(int-key)");
            return null;
        }

        private TemplateTableData _GetKeyData(string key)
        {
            if (dataFormatOptions == TableDataIndexFormat.StringDictionary)
            {
                TemplateTableData data = null;
                if (m_tableDataStringDictionary.TryGetValue(key, out data))
                {
                    return data;
                }
            }
            else Debug.LogError("[TemplateTable] TableDataIndexFormat: StringDictionary. Please use the GetKeyData(string-key)");
            return null;
        }

        private TemplateTableData _GetKeyData(int key1, int key2)
        {
            if (dataFormatOptions == TableDataIndexFormat.Int2IntDictionary)
            {
                Dictionary<int, TemplateTableData> dictionary = null;
                if (m_tableDataInt2IntDictionary.TryGetValue(key1, out dictionary))
                {
                    TemplateTableData data = null;
                    if (dictionary.TryGetValue(key2, out data))
                    {
                        return data;
                    }
                }
            }
            else Debug.LogError("[TemplateTable] TableDataIndexFormat: Int2IntDictionary. Please use the GetKeyData(int-key, int-key)");
            return null;
        }

        public static TemplateTableData GetIndexData(int index) { return Instance._GetIndexData(index); }
        public static TemplateTableData GetKeyData(int key) { return Instance._GetKeyData(key); }
        public static TemplateTableData GetKeyData(string key) { return Instance._GetKeyData(key); }
        public static TemplateTableData GetKeyData(int key1, int key2) { return Instance._GetKeyData(key1, key2); }
    }
}
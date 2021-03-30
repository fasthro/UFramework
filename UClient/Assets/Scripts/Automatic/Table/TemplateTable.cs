// UFramework Automatic.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFramework.Core;

namespace UFramework.Automatic
{
    public class TemplateTableData
    {
        /// <summary>
        /// 键值
        /// <summary>
        public int field1;

        /// <summary>
        /// 描述1
        /// <summary>
        public byte field2;

        /// <summary>
        /// 描述3
        /// <summary>
        public long field4;

        /// <summary>
        /// 描述4
        /// <summary>
        public float field5;

        /// <summary>
        /// 描述5
        /// <summary>
        public double field6;

        /// <summary>
        /// 描述6
        /// <summary>
        public bool field7;

        /// <summary>
        /// 描述7
        /// <summary>
        public string field8;

        /// <summary>
        /// 描述8
        /// <summary>
        public byte[] field9;

        /// <summary>
        /// 描述9
        /// <summary>
        public int[] field10;

        /// <summary>
        /// 多语言
        /// <summary>
        public long[] field11;

        /// <summary>
        /// 描述10
        /// <summary>
        public float[] field12;

        /// <summary>
        /// 描述11
        /// <summary>
        public double[] field13;

        /// <summary>
        /// 描述12
        /// <summary>
        public bool[] field14;

        /// <summary>
        /// 描述13
        /// <summary>
        public string[] field15;

        /// <summary>
        /// 描述14
        /// <summary>
        public Vector2[] field16;

        /// <summary>
        /// 描述15
        /// <summary>
        public Vector3[] field17;

        /// <summary>
        /// 描述16
        /// <summary>
        public LocalizationText field18;

        /// <summary>
        /// 描述17
        /// <summary>
        public LocalizationText[] field19;


    }

    public class TemplateTable : Singleton<TemplateTable>, ITableBehaviour
    {
        public string tableName => "Template";
        public int maxCount => m_tableDatas.Length;
        
        public TableKeyFormat dataFormatOptions = TableKeyFormat.Default;
        private TemplateTableData[] m_tableDatas;
        private Dictionary<int, TemplateTableData> m_tableDataIntDictionary;
        private Dictionary<string, TemplateTableData> m_tableDataStringDictionary;
        private Dictionary<int, Dictionary<int, TemplateTableData>> m_tableDataInt2IntDictionary;

        protected override void OnSingletonAwake()
        {
            
        }

        private TemplateTableData _GetWithIndex(int index)
        {
            if (dataFormatOptions == TableKeyFormat.Default)
            {
                if (index >= 0 && index < m_tableDatas.Length)
                {
                    return m_tableDatas[index];
                }
            }
            else Logger.Error($"Template table get data error! key/index > {index}");
            return null;
        }

        private TemplateTableData _GetWithKey(int key)
        {
            if (dataFormatOptions == TableKeyFormat.IntKey)
            {
                if (m_tableDataIntDictionary.TryGetValue(key, out var data))
                    return data;
            }
            else Logger.Error($"Template table get data error! key/index > {key}");
            return null;
        }

        private TemplateTableData _GetWithKey(string key)
        {
            if (dataFormatOptions == TableKeyFormat.StringKey)
            {
                if (m_tableDataStringDictionary.TryGetValue(key, out var data))
                    return data;
            }
            else Logger.Error($"Template table get data error! key/index > {key}");
            return null;
        }

        private TemplateTableData _GetWithKey(int key1, int key2)
        {
            if (dataFormatOptions == TableKeyFormat.Int2Key)
            {
                if (m_tableDataInt2IntDictionary.TryGetValue(key1, out var dictionary))
                {
                    if (dictionary.TryGetValue(key2, out var data))
                        return data;
                }
            }
            else Logger.Error($"Template table get data error! key/index > {key1},{key2}");
            return null;
        }

        public static TemplateTableData GetWithIndex(int index) { return Instance._GetWithIndex(index); }
        public static TemplateTableData GetWithKey(int key) { return Instance._GetWithKey(key); }
        public static TemplateTableData GetWithKey(string key) { return Instance._GetWithKey(key); }
        public static TemplateTableData GetWithKey(int key1, int key2) { return Instance._GetWithKey(key1, key2); }
    }
}
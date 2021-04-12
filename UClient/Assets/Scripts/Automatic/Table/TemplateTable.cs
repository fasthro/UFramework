// UFramework Automatic.
// 2021-04-13 12:29:36

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UFramework.Core;

namespace UFramework.Automatic
{
    [System.Serializable]
    public class TemplateTableData
    {
        /// <summary>
        /// 键值
        /// <summary>
        public int id;

        /// <summary>
        /// 描述1
        /// <summary>
        public byte f_byte;

        /// <summary>
        /// 描述3
        /// <summary>
        public long f_long;

        /// <summary>
        /// 描述4
        /// <summary>
        public float f_float;

        /// <summary>
        /// 描述5
        /// <summary>
        public double f_double;

        /// <summary>
        /// 描述6
        /// <summary>
        public bool f_bool;

        /// <summary>
        /// 描述7
        /// <summary>
        public string f_tring;

        /// <summary>
        /// 描述7
        /// <summary>
        public Vector2 f_vector2;

        /// <summary>
        /// 描述7
        /// <summary>
        public Vector3 f_vector3;

        /// <summary>
        /// 描述7
        /// <summary>
        public Color f_color;

        /// <summary>
        /// 描述7
        /// <summary>
        public Color32 f_color32;


    }
    
    [System.Serializable]
    public class TemplateTableDatas
    {
        private List<TemplateTableData> _datas = new List<TemplateTableData>();
        
        public void AddData(TemplateTableData data)
        {
            _datas.Add(data);
        }
        
        public List<TemplateTableData> GetDatas()
        {
            return _datas;
        }
    
        public static List<TemplateTableData> Load()
        {
            var path = IOPath.PathCombine(UApplication.TableDirectory, $"Template.bytes");
            var obj = TableSerialize.Deserialize<TemplateTableDatas>(path);
            return obj._datas;
        }
    }

    public class TemplateTable : Singleton<TemplateTable>
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
            var datas = TemplateTableDatas.Load();
            switch (dataFormatOptions)
            {
                case TableKeyFormat.Default:
                    m_tableDatas = datas.ToArray();
                    break;
                case TableKeyFormat.IntKey:
                    m_tableDataIntDictionary = new Dictionary<int, TemplateTableData>();
                    for (var i = 0; i < datas.Count; i++)
                    {
                        var data = datas[i];
                        if (!m_tableDataIntDictionary.ContainsKey(data.id))
                        {
                            m_tableDataIntDictionary.Add(data.id, data);
                        }
                    }

                    break;
                case TableKeyFormat.StringKey:
                    m_tableDataStringDictionary = new Dictionary<string, TemplateTableData>();
                    for (var i = 0; i < datas.Count; i++)
                    {
                        var data = datas[i];
                        var id = data.id.ToString();
                        if (!m_tableDataStringDictionary.ContainsKey(id))
                            m_tableDataStringDictionary.Add(id, data);
                    }

                    break;
                case TableKeyFormat.Int2Key:
                    m_tableDataInt2IntDictionary = new Dictionary<int, Dictionary<int, TemplateTableData>>();
                    for (var i = 0; i < datas.Count; i++)
                    {
                        var data = datas[i];
                        if (!m_tableDataInt2IntDictionary.ContainsKey(data.id))
                            m_tableDataInt2IntDictionary.Add(data.id, new Dictionary<int, TemplateTableData>());

                        if (!m_tableDataInt2IntDictionary[data.id].ContainsKey(data.f_byte))
                            m_tableDataInt2IntDictionary[data.id].Add(data.f_byte, data);
                    }

                    break;
            }
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
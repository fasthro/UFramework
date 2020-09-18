/*
 * @Author: fasthro
 * @Date: 2019-12-19 17:11:35
 * @Description: table parse
 */
using System.Collections.Generic;
using UFramework.Asset;
using UnityEngine;

namespace UFramework.Table
{
    public abstract class TableParse
    {
        protected string m_tableName;
        protected FormatOptions m_format;
        protected string m_content;

        public TableParse(string tableName, FormatOptions format) { m_tableName = tableName; }

        protected void LoadAsset()
        {
            if (!string.IsNullOrEmpty(m_content)) return;
            var filePath = IOPath.PathCombine(App.TableDataDirectory, m_tableName + ".csv");
#if UNITY_EDITOR
            m_content = IOPath.FileReadText(filePath);
#else
            var loader = BundleLoader.AllocateAsset(filePath);
            var ready = loader.LoadSync();
            if (ready)
            {
                var ta = loader.asset.GetAsset<TextAsset>();
                if (ta != null)
                {
                    m_content = ta.text;
                }
            }
            loader.Unload(true);
#endif
        }

        public abstract T[] ParseArray<T>();
        public abstract Dictionary<string, T> ParseStringDictionary<T>();
        public abstract Dictionary<int, T> ParseIntDictionary<T>();
        public abstract Dictionary<int, Dictionary<int, T>> ParseInt2IntDictionary<T>();
    }
}
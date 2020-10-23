/*
 * @Author: fasthro
 * @Date: 2019-12-19 17:11:35
 * @Description: table parse
 */
using System.Collections.Generic;
using UFramework.Assets;
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
            var filePath = IOPath.PathCombine(App.AssetsDirectory, "Table", "Data", m_tableName + ".csv");
#if UNITY_EDITOR
            m_content = IOPath.FileReadText(filePath);
#else
            var asset = Asset.LoadAsset(filePath, typeof(TextAsset));
            m_content = asset.GetAsset<TextAsset>().text;
            asset.Unload();
#endif
        }

        public abstract T[] ParseArray<T>();
        public abstract Dictionary<string, T> ParseStringDictionary<T>();
        public abstract Dictionary<int, T> ParseIntDictionary<T>();
        public abstract Dictionary<int, Dictionary<int, T>> ParseInt2IntDictionary<T>();
    }
}
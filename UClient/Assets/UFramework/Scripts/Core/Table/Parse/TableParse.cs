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
        protected string _tableName;
        protected FormatOptions _format;
        protected string _content;

        public TableParse(string tableName, FormatOptions format) { _tableName = tableName; }

        protected void LoadAsset()
        {
            if (!string.IsNullOrEmpty(_content)) return;
            var filePath = IOPath.PathCombine(App.AssetsDirectory, "Table", "Data", _tableName + ".csv");
#if UNITY_EDITOR
            _content = IOPath.FileReadText(filePath);
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
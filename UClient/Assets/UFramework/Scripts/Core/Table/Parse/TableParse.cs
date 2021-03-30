/*
 * @Author: fasthro
 * @Date: 2019-12-19 17:11:35
 * @Description: table parse
 */
using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Core
{
    public abstract class TableParse
    {
        protected readonly string _tableName;
        protected string _content;

        public TableParse(string tableName) { _tableName = tableName; }

        protected void LoadAsset()
        {
            if (!string.IsNullOrEmpty(_content)) return;
            var filePath = IOPath.PathCombine(UApplication.AssetsDirectory, "Table", "Data", _tableName + ".csv");
#if UNITY_EDITOR
            _content = IOPath.FileReadText(filePath);
#else
            var asset = Assets.LoadAsset(filePath, typeof(TextAsset));
            _content = asset.GetAsset<TextAsset>().text;
            asset.Unload();
#endif
        }

        public abstract T[] Parse<T>();
        public abstract Dictionary<string, T> ParseStringKey<T>();
        public abstract Dictionary<int, T> ParseIntKey<T>();
        public abstract Dictionary<int, Dictionary<int, T>> ParseInt2Key<T>();
    }
}
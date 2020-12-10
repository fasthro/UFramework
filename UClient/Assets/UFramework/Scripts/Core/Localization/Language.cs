/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:37:22
 * @Description: 国际化语言
 */
using System.Collections.Generic;
using UFramework.Assets;
using UnityEngine;

namespace UFramework.Localization
{
    public class Language : Singleton<Language>
    {
        static string DataPath;

        private SystemLanguage _langType;
        private string _langTypeStr;

        private Dictionary<int, string[]> _modelDict = new Dictionary<int, string[]>();

        protected override void OnSingletonAwake()
        {
            DataPath = IOPath.PathCombine(App.AssetsDirectory, "Language", "Data");

            var app = Serialize.Serializable<AppSerdata>.Instance;
            if (app.useSystemLanguage)
            {
                _langType = Application.systemLanguage;
            }

            if (!app.supportedLanguages.Contains(_langType))
            {
                _langType = app.defaultLanguage;
            }

            _langTypeStr = _langType.ToString();
        }

        /// <summary>
        /// 获取语言文本
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        private string _Get(int model, int key)
        {
            string[] ls = null;
            if (_modelDict.TryGetValue(model, out ls))
            {
                if (key >= 0 && key < ls.Length)
                {
                    return ls[key];
                }
            }
            else
            {
                _Load(model);
                if (_modelDict.TryGetValue(model, out ls))
                {
                    if (key >= 0 && key < ls.Length)
                    {
                        return ls[key];
                    }
                }
            }
            // TODO 编辑器模式最好输出 model 和 key 对应在表里的名字
            return string.Empty;
        }

        /// <summary>
        /// 加载模块
        /// </summary>
        /// <param name="model"></param>
        void _Load(int model)
        {
            if (_modelDict.ContainsKey(model)) return;

            string text = "";
            string filePath = IOPath.PathCombine(DataPath, _langTypeStr, model + ".txt");
#if UNITY_EDITOR
            text = IOPath.FileReadText(filePath);
#else
            var asset = Asset.LoadAsset(filePath, typeof(TextAsset));
            text = asset.GetAsset<TextAsset>().text;
            asset.Unload();
#endif
            _modelDict.Add(model, text.Split('\n'));
        }

        /// <summary>
        /// 获取多语言内容
        /// </summary>
        /// <param name="model"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(int model, int key)
        {
            return Instance._Get(model, key);
        }

        /// <summary>
        /// 预加载多语言模块
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static void Preload(int model)
        {
            Instance._Load(model);
        }
    }
}
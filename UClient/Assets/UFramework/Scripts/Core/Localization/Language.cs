/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:37:22
 * @Description: 国际化语言
 */
using System.Collections.Generic;
using UFramework.Asset;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Localization
{
    public class Language : Singleton<Language>
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        /// <value></value>
        private SystemLanguage m_languageType;

        /// <summary>
        /// 语言类型文本
        /// </summary>
        private string m_languageTypeString;


        /// <summary>
        /// 语言缓存
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="string[]"></typeparam>
        /// <returns></returns>
        private Dictionary<int, string[]> m_modelDictonary = new Dictionary<int, string[]>();

        protected override void OnSingletonAwake()
        {
            var app = UConfig.Read<AppConfig>();
            if (app.useSystemLanguage)
            {
                m_languageType = Application.systemLanguage;
            }

            if (!app.supportedLanguages.Contains(m_languageType))
            {
                m_languageType = app.defaultLanguage;
            }

            m_languageTypeString = m_languageType.ToString();
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
            if (m_modelDictonary.TryGetValue(model, out ls))
            {
                if (key >= 0 && key < ls.Length)
                {
                    return ls[key];
                }
            }
            else
            {
                _Load(model);
                if (m_modelDictonary.TryGetValue(model, out ls))
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
            if (m_modelDictonary.ContainsKey(model)) return;

            string text = "";
            string dataPath = IOPath.PathCombine(App.LanguageDataDirectory, m_languageTypeString, model + ".txt");
#if UNITY_EDITOR
            text = IOPath.FileReadText(dataPath);
#else
            var loader = BundleLoader.AllocateAsset(dataPath);
            var ready = loader.LoadSync();
            if (ready)
            {
                var ta = loader.asset.GetAsset<TextAsset>();
                if (ta != null)
                {
                    text = ta.text;
                }
            }
            loader.Unload();
#endif
            m_modelDictonary.Add(model, text.Split('\n'));
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
/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:37:22
 * @Description: 国际化语言
 */
using System.Collections.Generic;
using UFramework.Config;
using UFramework.ResLoader;
using UnityEngine;

namespace UFramework.i18n
{
    public class Language : MonoSingleton<Language>
    {
        /// <summary>
        /// 语言类型
        /// </summary>
        /// <value></value>
        static SystemLanguage languageType;

        /// <summary>
        /// 语言类型文本
        /// </summary>
        static string languageTypeString;


        /// <summary>
        /// 语言缓存
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="string[]"></typeparam>
        /// <returns></returns>
        static Dictionary<int, string[]> modelDictonary = new Dictionary<int, string[]>();

        protected override void OnSingletonStart()
        {
            var app = UConfig.Read<AppConfig>();
            if (app.useSystemLanguage)
            {
                languageType = Application.systemLanguage;
            }

            if (!app.supportedLanguages.Contains(languageType))
            {
                languageType = app.defaultLanguage;
            }

            languageTypeString = languageType.ToString();
        }

        /// <summary>
        /// 获取语言文本
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string Get(int model, int key)
        {
            string[] ls = null;
            if (modelDictonary.TryGetValue(model, out ls))
            {
                if (key >= 0 && key < ls.Length)
                {
                    return ls[key];
                }
            }
            else
            {
                BuildModel(model);
                if (modelDictonary.TryGetValue(model, out ls))
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
        /// 构建模块
        /// </summary>
        /// <param name="model"></param>
        static void BuildModel(int model)
        {
            if (modelDictonary.ContainsKey(model)) return;

            string text = "";
            string dataPath = IOPath.PathCombine(App.LanguageDataDirectory, languageTypeString, model + ".txt");
#if UNITY_EDITOR
            text = IOPath.FileReadText(dataPath);
#else
            var resLoader = AssetBundleLoader.AllocateRes(dataPath);
            var ready = resLoader.LoadSync();
            if (ready)
            {
                var ta = resLoader.bundleAssetRes.GetAsset<TextAsset>();
                if (ta != null)
                {
                    text = ta.text;
                }
            }
            resLoader.Unload();
#endif
            modelDictonary.Add(model, text.Split('\n'));
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-07-01 20:01:09
 * @Description: Language Page
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.Localization
{
    public class LocalizationPage : IPage, IPageBar
    {
        public string menuName { get { return "Localization"; } }

        static AppConfig Config { get { return Core.Serializer<AppConfig>.Instance; } }

        /// <summary>
        /// 是否使用系统语言
        /// </summary>
        [BoxGroup("General Settings")]
        [LabelText("    Use System Language")]
        public bool useSystemLanguage;

        /// <summary>
        /// 默认语言
        /// </summary>
        [BoxGroup("General Settings")]
        [LabelText("    Default Language")]
        public SystemLanguage defaultLanguage = SystemLanguage.ChineseSimplified;

        /// <summary>
        /// 支持的语言列表
        /// </summary>
        [ListDrawerSettings(Expanded = true)]
        public List<SystemLanguage> supportedLanguages = new List<SystemLanguage>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            bool hasNew = false;
            if (Config.supportedLanguages.Count == 0)
            {
                hasNew = true;
                Config.supportedLanguages.Add(Config.defaultLanguage);
            }

            useSystemLanguage = Config.useSystemLanguage;
            defaultLanguage = Config.defaultLanguage;
            supportedLanguages = Config.supportedLanguages;

            if (hasNew)
            {
                Config.Serialize();
            }
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate")))
            {
                var opt = new ExcelReaderOptions();
                opt.languages = Config.supportedLanguages;
                var reader = new ExcelReader(opt);
                reader.Read();

                new Excel2Text(reader);
                new Excel2Index(reader);
                // TODO 实现多语言Lua Index
                // new Excel2LuaIndex(reader);

                AssetDatabase.Refresh();
            }
        }

        public void OnSaveDescribe()
        {
            if (Config == null) return;
            Config.useSystemLanguage = useSystemLanguage;
            Config.defaultLanguage = defaultLanguage;
            Config.supportedLanguages = supportedLanguages;
            Config.Serialize();
        }
    }
}
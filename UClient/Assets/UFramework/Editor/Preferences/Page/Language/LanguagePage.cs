/*
 * @Author: fasthro
 * @Date: 2020-07-01 20:01:09
 * @Description: Language Page
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Language;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class LanguagePage : IPage, IPageBar
    {
        public string menuName { get { return "Language"; } }

        static AppSerdata Serdata { get { return Serialize.Serializable<AppSerdata>.Instance; } }

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
            if (Serdata.supportedLanguages.Count == 0)
            {
                hasNew = true;
                Serdata.supportedLanguages.Add(Serdata.defaultLanguage);
            }

            useSystemLanguage = Serdata.useSystemLanguage;
            defaultLanguage = Serdata.defaultLanguage;
            supportedLanguages = Serdata.supportedLanguages;

            if (hasNew)
            {
                Serdata.Serialization();
            }
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate")))
            {
                var opt = new ExcelReaderOptions();
                opt.languages = Serdata.supportedLanguages;
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
            if (Serdata == null) return;
            Serdata.useSystemLanguage = useSystemLanguage;
            Serdata.defaultLanguage = defaultLanguage;
            Serdata.supportedLanguages = supportedLanguages;
            Serdata.Serialization();
        }
    }
}
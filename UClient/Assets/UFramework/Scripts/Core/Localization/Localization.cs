/*
 * @Author: fasthro
 * @Date: 2019-11-11 11:37:22
 * @Description: 国际化
 */
using System.Collections.Generic;
using UnityEngine;

namespace UFramework.Core
{
    public class Localization : Singleton<Localization>
    {
        static string LanguageDataPath;
        static Dictionary<int, string[]> _moduleDict = new Dictionary<int, string[]>();

        public static SystemLanguage CurLanguage { get; private set; }

        static int CurModule;

        protected override void OnSingletonAwake()
        {
            var app = Core.Serializer<AppConfig>.Instance;
            if (app.useSystemLanguage)
                CurLanguage = Application.systemLanguage;

            if (!app.supportedLanguages.Contains(CurLanguage))
                CurLanguage = app.defaultLanguage;

            LanguageDataPath = IOPath.PathCombine(UApplication.AssetsDirectory, "Localization", "Language", CurLanguage.ToString());
        }

        public static void SetTextModule(int module)
        {
            CurModule = module;
        }

        public static void LoadTextModule(int module)
        {
            if (_moduleDict.ContainsKey(module)) return;

            string text = "";
            string filePath = IOPath.PathCombine(LanguageDataPath, module + ".txt");
#if UNITY_EDITOR
            text = IOPath.FileReadText(filePath);
#else
            var asset = Asset.LoadAsset(filePath, typeof(TextAsset));
            text = asset.GetAsset<TextAsset>().text;
            asset.Unload();
#endif
            _moduleDict.Add(module, text.Split('\n'));
        }

        public static string GetText(int key)
        {
            return GetText(CurModule, key);
        }

        public static string GetText(int module, int key)
        {
            string[] lines = null;
            if (_moduleDict.TryGetValue(module, out lines))
            {
                if (key >= 0 && key < lines.Length)
                    return lines[key];
            }
            else
            {
                LoadTextModule(module);
                return GetText(module, key);
            }
            return string.Empty;
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-06-28 14:01:30
 * @Description: framework preferences window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UFramework.Editor.Preferences.Assets;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class PreferencesWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Preferences", false, 500)]
        private static void OpenWindow()
        {
            var window = GetWindow<PreferencesWindow>();
            window.titleContent = new GUIContent("Preferences");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
        }

        protected override void OnInitialize()
        {
            AddPage(new WelcomePage());
            AddPage(new AssetBundlePage());
            AddPage(new AssetBundlePreferencesPage());
            AddPage(new AssetImporterPage());
            AddPage(new AssetImporterPreferencesPage());
            AddPage(new ConfigPage());
            AddPage(new LanguagePage());
            AddPage(new LuaPage());
            AddPage(new SDKPage());
            AddPage(new TablePage());
            AddPage(new ProtoPage());
            AddPage(new ProjrectPage());
        }
    }
}


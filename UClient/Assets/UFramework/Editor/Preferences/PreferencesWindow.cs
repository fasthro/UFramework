/*
 * @Author: fasthro
 * @Date: 2020-06-28 14:01:30
 * @Description: framework preferences window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class PreferencesWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Preferences", false, 1000)]
        private static void OpenWindow()
        {
            var window = GetWindow<PreferencesWindow>();
            window.titleContent = new GUIContent("Preferences");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        protected override void OnInitialize()
        {
            AddPage(new AssetBundlePage());
            AddPage(new AssetBundlePreferencesPage());
            AddPage(new ConfigPage());
            AddPage(new TablePage());
            AddPage(new LanguagePage());
            AddPage(new SDKPage());
            AddPage(new LuaPage());
        }
    }
}

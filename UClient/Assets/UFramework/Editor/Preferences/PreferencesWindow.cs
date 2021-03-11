// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-28 14:01:30
// * @Description:
// --------------------------------------------------------------------------------

using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UFramework.Editor.Preferences.AssetBundle;
using UFramework.Editor.Preferences.AssetImporter;
using UFramework.Editor.Preferences.BuildFiles;
using UFramework.Editor.Preferences.Consoles;
using UFramework.Editor.Preferences.Localization;
using UFramework.Editor.Preferences.Lua;
using UFramework.Editor.Preferences.Projrect;
using UFramework.Editor.Preferences.Proto;
using UFramework.Editor.Preferences.Serializable;
using UFramework.Editor.Preferences.Table;
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
            AddPage(new BuildFilesPage());
            AddPage(new ConsolePage(this));
            AddPage(new SerializePage());
            AddPage(new LocalizationPage());
            AddPage(new LuaPage());
            AddPage(new TablePage());
            AddPage(new ProtoPage());
            AddPage(new ProjectPage());
        }
    }
}
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
using UFramework.Editor.Preferences.Page.Table;
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
        private static readonly string CompileCallback_PrefKey = "PreferencesWindow_Callback_Page";
        private static PreferencesWindow instance;
        private static bool isCallback;

        [MenuItem("UFramework/Preferences", false, 500)]
        private static void OpenWindow()
        {
            instance = GetWindow<PreferencesWindow>();
            instance.titleContent = new GUIContent("Preferences");
            instance.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
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

        public static void RsegisterCallbackWithEditorCompile(IPage page)
        {
            SetPref(CompileCallback_PrefKey, page.menuName);
            Helper.EditorCompileComplete.Rsegister<PreferencesWindow>("OnEditorCompileCallback");
        }

        static void OnEditorCompileCallback()
        {
            if (instance == null)
                OpenWindow();

            isCallback = true;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (isCallback && instance.page != null)
            {
                if (instance.page.menuName.Equals(GetPrefValue(CompileCallback_PrefKey)))
                    ((IPageCallback) instance.page)?.OnEdittorCompileCallback();
                isCallback = false;
                SetPref(CompileCallback_PrefKey, string.Empty);
            }
        }

        static void SetPref(string key, string value)
        {
            EditorPrefs.SetString(key, value);
        }

        static string GetPrefValue(string key)
        {
            return EditorPrefs.HasKey(key) ? EditorPrefs.GetString(key) : "";
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-06-28 14:01:30
 * @Description: framework preferences window
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class Preferences : OdinMenuWindow
    {
        [MenuItem("UFramework/Preferences")]
        private static void OpenWindow()
        {
            var window = GetWindow<Preferences>();
            window.titleContent = new GUIContent("UFramework Preferences");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        protected override void OnInitialize()
        {
            AddPage(new AssetBundlePage());
            AddPage(new AssetBundlePreferencesPage());
            AddPage(new ConfigPage());
            AddPage(new TablePage());
            AddPage(new TablePreferencesPage());
            AddPage(new LanguagePage());
            AddPage(new SDKPage());
            AddPage(new LuaPage());
            AddPage(new WrapPage());
        }
    }
}

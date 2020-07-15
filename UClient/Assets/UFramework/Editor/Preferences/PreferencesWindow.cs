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
    public class Preferences : OdinMenuEditorWindow
    {
        [MenuItem("UFramework/Preferences")]
        private static void OpenWindow()
        {
            var window = GetWindow<Preferences>();
            window.titleContent = new GUIContent("UFramework Preferences");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        static AssetBundlePage assetBundlePage = new AssetBundlePage();
        static AssetBundlePreferencesPage assetBundlePreferencesPage = new AssetBundlePreferencesPage();
        static ConfigPage configPage = new ConfigPage();
        static TablePage tablePage = new TablePage();
        static TablePreferencesPage tablePreferencesPage = new TablePreferencesPage();
        static LanguagePage languagePage = new LanguagePage();

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(false);

            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            // tree.Add("Lua", this);
            tree.Add(assetBundlePage.menuName, assetBundlePage.GetInstance());
            tree.Add(assetBundlePreferencesPage.menuName, assetBundlePreferencesPage.GetInstance());
            // config
            tree.Add(configPage.menuName, configPage.GetInstance());
            // table
            tree.Add(tablePage.menuName, tablePage.GetInstance());
            tree.Add(tablePreferencesPage.menuName, tablePreferencesPage.GetInstance());
            // language
            tree.Add(languagePage.menuName, languagePage.GetInstance());
            // sdk
            // tree.Add("SDK", this);
            // tree.Add("Version", this);
            // tree.Add("Other", this);

            tree.Selection.SelectionChanged += SelectionChanged;

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            if (this.MenuTree == null) return;
            var selection = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                var page = selection.Value as IPage;
                if (page != null)
                {
                    GUILayout.Label(page.menuName);
                    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Apply")))
                    {
                        page.OnApply();
                    }
                    page.OnDrawFunctoinButton();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        void SelectionChanged(SelectionChangedType type)
        {
            if (type == SelectionChangedType.ItemAdded)
            {
                var selection = this.MenuTree.Selection.FirstOrDefault();
                var page = selection.Value as IPage;
                page.OnRenderBefore();
            }
        }
    }
}

/*
 * @Author: fasthro
 * @Date: 2020-07-04 09:44:10
 * @Description: framework runtime window
 */
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Runtime
{
    public class RuntimeWindow : OdinMenuEditorWindow
    {
        [MenuItem("UFramework/Runtime")]
        private static void OpenWindow()
        {
            var window = GetWindow<RuntimeWindow>();
            window.titleContent = new GUIContent("UFramework Runtime");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        ResLoaderRuntimePage resLoaderRuntimePage = new ResLoaderRuntimePage();

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(false);

            tree.DefaultMenuStyle.IconSize = 28.00f;
            tree.Config.DrawSearchToolbar = true;

            tree.Add(resLoaderRuntimePage.menuName, resLoaderRuntimePage.GetInstance());

            tree.Selection.SelectionChanged += SelectionChanged;

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selection = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                var page = selection.Value as IPage;
                if (page != null)
                {
                    GUILayout.Label(page.menuName);
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
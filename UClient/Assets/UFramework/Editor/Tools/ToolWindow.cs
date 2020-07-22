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

namespace UFramework.Editor.Tool
{
    public class ToolWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Tools")]
        private static void OpenWindow()
        {
            var window = GetWindow<ToolWindow>();
            window.titleContent = new GUIContent("UFramework Tools");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        protected override void OnInitialize()
        {
            AddPage(new LuaPage());
            AddPage(new SvnPage());
        }
    }
}
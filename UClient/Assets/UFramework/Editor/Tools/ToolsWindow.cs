/*
 * @Author: fasthro
 * @Date: 2020-07-23 01:41:56
 * @Description: version control window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Tools
{
    public class ToolsWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Tools", false, 400)]
        private static void OpenWindow()
        {
            var window = GetWindow<ToolsWindow>();
            window.titleContent = new GUIContent("Tools");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
        }

        protected override void OnInitialize()
        {
            AddPage(new SerializablePage());
        }
    }
}
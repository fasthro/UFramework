/*
 * @Author: fasthro
 * @Date: 2020-07-23 01:41:56
 * @Description: version control window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Version
{
    public class VersionControlWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Version Control")]
        private static void OpenWindow()
        {
            var window = GetWindow<VersionControlWindow>();
            window.titleContent = new GUIContent("UFramework Version Contorl");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        protected override void OnInitialize()
        {
            drawSearchToolbar = false;
        }
    }
}
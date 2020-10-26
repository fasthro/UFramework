/*
 * @Author: fasthro
 * @Date: 2020-07-23 01:41:56
 * @Description: version control window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class VersionControlWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Version Control", false, 100)]
        private static void OpenWindow()
        {
            var window = GetWindow<VersionControlWindow>();
            window.titleContent = new GUIContent("Version Contorl");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
        }

        protected override void OnInitialize()
        {
            AddPage(new AppPage());
            AddPage(new VersionPage());
            AddPage(new VersionSerializablePage());
            AddPage(new BuilderPage());
            AddPage(new BuildSettingPage());
        }
    }
}
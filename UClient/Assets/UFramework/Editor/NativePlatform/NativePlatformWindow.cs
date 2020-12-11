/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:12:24
 * @Description: Native Platform Window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.NativePlatform
{
    public class NativePlatformWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Native Platform", false, 300)]
        private static void OpenWindow()
        {
            var window = GetWindow<NativePlatformWindow>();
            window.titleContent = new GUIContent("Native Platform");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
        }

        protected override void OnInitialize()
        {
            AddPage(new AndroidPage());
        }
    }
}

/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:12:24
 * @Description: Native Window
 */
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Native
{
    public class NativeWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Native", false, 300)]
        private static void OpenWindow()
        {
            var window = GetWindow<NativeWindow>();
            window.titleContent = new GUIContent("Native");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1280, 720);
        }

        protected override void OnInitialize()
        {
            AddPage(new AndroidPage());
        }
    }
}

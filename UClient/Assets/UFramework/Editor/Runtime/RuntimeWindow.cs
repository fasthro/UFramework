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
    public class RuntimeWindow : OdinMenuWindow
    {
        [MenuItem("UFramework/Runtime")]
        private static void OpenWindow()
        {
            var window = GetWindow<RuntimeWindow>();
            window.titleContent = new GUIContent("UFramework Runtime");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }

        protected override void OnInitialize()
        {
            AddPage(new ResLoaderRuntimePage());
        }
    }
}
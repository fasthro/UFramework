/*
 * @Author: fasthro
 * @Date: 2019-11-26 20:02:16
 * @Description: editor window base class
 */
 
using UnityEditor;

namespace UFramework.UEditor
{
    public abstract class UEditorWindow : EditorWindow
    {
        public void Initialize() { OnInitialize(); }
        
        protected virtual void OnInitialize() { }

        void OnEnable() { Initialize(); }

        public static T ShowWindow<T>() where T : UEditorWindow, new()
        {
            var window = GetWindow<T>(false, "");
            window.Initialize();
            return window;
        }
    }
}
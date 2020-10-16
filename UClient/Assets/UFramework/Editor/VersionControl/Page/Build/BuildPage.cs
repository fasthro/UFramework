/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: build
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class BuilderPage : IPage, IPageBar
    {
        public string menuName { get { return "Build"; } }
        static VersionControl_VersionConfig describeObject;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<VersionControl_VersionConfig>();
        }

        public void OnSaveDescribe()
        {
            describeObject.Save();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Application")))
            {
                
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Patch")))
            {
                
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Assets")))
            {
                
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Scripts")))
            {
                
            }
        }
    }
}
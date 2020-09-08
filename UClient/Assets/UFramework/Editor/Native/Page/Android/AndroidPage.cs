/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:14:38
 * @Description: Android Page
 */
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Editor.Native
{
    public class AndroidPage : IPage
    {
        public string menuName { get { return "Android"; } }
        static AndroidNativeConfig describeObject;

        /// <summary>
        /// module
        /// </summary>
        [ShowInInspector]
        [ListDrawerSettings(OnTitleBarGUI = "OnUpdateAllAARTitleBarGUI")]
        [LabelText("Modules AAR")]
        public List<AndroidNativeModule> modules = new List<AndroidNativeModule>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AndroidNativeConfig>();
            modules.Clear();
            for (int i = 0; i < describeObject.modules.Length; i++) { }
            modules.AddRange(describeObject.modules);
        }

        public void OnSaveDescribe()
        {
            describeObject.modules = modules.ToArray();
            describeObject.Save();
        }

        private void OnUpdateAllAARTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                UpdateAllAAR(false);
            }
        }

        /// <summary>
        /// Update All AAR
        /// </summary>
        /// <param name="isDebug"></param>
        private void UpdateAllAAR(bool isDebug)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                UpdateAAR(isDebug, modules[i].name);
            }
        }

        /// <summary>
        /// Update Modul ARR
        /// </summary>
        /// <param name="isDebug"></param>
        /// <param name="moduleName"></param>
        public static void UpdateAAR(bool isDebug, string moduleName)
        {
            var suffix = isDebug ? "-debug.aar" : "-release.aar";
            var libsRoot = IOPath.PathCombine(App.AndroidNativeDirectory, moduleName, "libs");

            var sourceRoot = IOPath.PathCombine(App.AndroidNativeDirectory, moduleName, "build/outputs/aar/");
            var sourceFile = IOPath.PathCombine(sourceRoot, moduleName + suffix);

            var destRoot = IOPath.PathCombine(Application.dataPath, "UFramework/Plugins/Android/libs");
            var destFile = IOPath.PathCombine(destRoot, "uframework-" + moduleName + ".aar");

            if (IOPath.DirectoryExists(sourceRoot))
            {
                IOPath.FileDelete(IOPath.PathCombine(destRoot, "uframework-" + moduleName + ".aar"));
                IOPath.FileCopy(sourceFile, destFile);
                Debug.Log("Android Native Update [" + moduleName + "] AAR Succeed.");
            }
            else
            {
                Debug.LogError("Android Native Update AAR Failled. [" + moduleName + "] Module AAR File Not Exist.");
            }
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:14:38
 * @Description: Android Page
 */
using System;
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

        // 工程路径
        [HideInInspector]
        public static string ProjectPath;

        /// <summary>
        /// module
        /// </summary>
        [ShowInInspector]
        [ListDrawerSettings(Expanded = true, OnTitleBarGUI = "OnUpdateAllAARTitleBarGUI")]
        [LabelText("Module AAR")]
        public List<AndroidNativeModule> modules = new List<AndroidNativeModule>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            ProjectPath = IOPath.PathCombine(Environment.CurrentDirectory, "Native", "Android");

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
            var libsRoot = IOPath.PathCombine(ProjectPath, moduleName, "libs");

            var sourceRoot = IOPath.PathCombine(ProjectPath, moduleName, "build/outputs/aar/");
            var sourceFile = IOPath.PathCombine(sourceRoot, moduleName + suffix);

            var destRoot = IOPath.PathCombine(Application.dataPath, "UFramework/Plugins/Android/libs");
            var destFile = IOPath.PathCombine(destRoot, "uframework-" + moduleName + ".aar");

            if (IOPath.DirectoryExists(sourceRoot))
            {
                IOPath.FileDelete(IOPath.PathCombine(destRoot, "uframework-" + moduleName + ".aar"));
                IOPath.FileCopy(sourceFile, destFile);
                Logger.Debug("Android Native Update [" + moduleName + "] AAR Succeed.");
            }
            else
            {
                Logger.Error("Android Native Update AAR Failled. [" + moduleName + "] Module AAR File Not Exist.");
            }
        }
    }
}
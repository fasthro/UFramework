/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:14:38
 * @Description: Android Page
 */
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Editor.NativePlatform
{
    public class AndroidPage : IPage
    {
        public string menuName { get { return "Android"; } }
        static NativePlatform_AndroidConfig Config { get { return Serializer<NativePlatform_AndroidConfig>.Instance; } }

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

            modules.Clear();
            for (int i = 0; i < Config.modules.Length; i++) { }
            modules.AddRange(Config.modules);
        }

        public void OnSaveDescribe()
        {
            Config.modules = modules.ToArray();
            Config.Serialize();
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
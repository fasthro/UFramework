/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:14:38
 * @Description: Android Page
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        public List<string> modules = new List<string>();

        [Button]
        [HorizontalGroup("AAR")]
        [LabelText("Update AAR Debug")]
        private void CopyAARDebug() { CopyAAR(true); }

        [Button]
        [HorizontalGroup("AAR")]
        [LabelText("Update AAR Release")]
        private void CopyAARRelease() { CopyAAR(false); }

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AndroidNativeConfig>();
            modules.Clear();
            modules.AddRange(describeObject.modules);
        }

        public void OnSaveDescribe()
        {
            describeObject.modules = modules.ToArray();
            describeObject.Save();
        }

        /// <summary>
        /// Copy AAR
        /// </summary>
        /// <param name="isDebug"></param>
        private void CopyAAR(bool isDebug)
        {
            var suffix = isDebug ? "-debug.aar" : "-release.aar";
            for (int i = 0; i < modules.Count; i++)
            {
                var moduleName = modules[i];

                var sourceRoot = IOPath.PathCombine(App.AndroidNativeDirectory, moduleName, "build/outputs/aar/");
                var sourceFile = IOPath.PathCombine(sourceRoot, moduleName + suffix);

                var destRoot = IOPath.PathCombine(Application.dataPath, "UFramework/Plugins/Android/libs");
                var destFile = IOPath.PathCombine(destRoot, moduleName + ".aar");

                if (!IOPath.DirectoryExists(sourceRoot))
                {
                    Debug.LogError("Android Native Update AAR Failled. [" + moduleName + "] Module AAR File Not Exist.");
                    continue;
                }

                IOPath.FileDelete(IOPath.PathCombine(destRoot, "uframework-" + moduleName + ".aar"));
                IOPath.FileCopy(sourceFile, destFile);
            }
        }
    }
}
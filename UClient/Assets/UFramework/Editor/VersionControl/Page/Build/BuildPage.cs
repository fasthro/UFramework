/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: build
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl.Build
{
    public class BuilderPage : IPage, IPageBar
    {
        public string menuName { get { return "Build"; } }
        static VersionControl_Build_Config Config { get { return Core.Serializer<VersionControl_Build_Config>.Instance; } }

        /// <summary>
        /// 版本
        /// </summary>
        [OnValueChanged("OnSaveDescribe")]
        [DisableIf("_isBuild")]
        [LabelText("App Version (Only Display)")]
        public string appVersion;

        [ShowInInspector, HideLabel]
        public BuildPageBuildTable buildTable = new BuildPageBuildTable();

        private bool _isBuild { get { return buildTable.isBuild; } }

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            appVersion = Config.appVersion;
            if (string.IsNullOrEmpty(appVersion))
                appVersion = "1.0.0";

            ApplySetting();
        }

        public void OnSaveDescribe()
        {
            Config.appVersion = appVersion;
            Config.Serialize();
        }

        public void OnPageBarDraw()
        {

        }

        private void ApplySetting()
        {
            PlayerSettings.Android.useAPKExpansionFiles = Config.packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB;
            EditorUserBuildSettings.buildAppBundle = Config.packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE;

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, Config.scripting);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, Config.scripting);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, Config.scripting);

            // arm64
            var _googlePlaySupportARM64 = Config.scripting == ScriptingImplementation.IL2CPP && (Config.packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE || Config.packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB);
            if (_googlePlaySupportARM64)
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            }
            else
            {
                if (Config.supportARM64)
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                else
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
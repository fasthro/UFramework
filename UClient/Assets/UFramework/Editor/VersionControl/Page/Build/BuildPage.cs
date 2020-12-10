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

namespace UFramework.Editor.VersionControl
{
    public class BuilderPage : IPage, IPageBar
    {
        public string menuName { get { return "Build"; } }
        static VersionBuildSerdata Serdata { get { return Serialize.Serializable<VersionBuildSerdata>.Instance; } }

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
            appVersion = Serdata.appVersion;
            if (string.IsNullOrEmpty(appVersion))
                appVersion = "1.0.0";

            ApplySetting();
        }

        public void OnSaveDescribe()
        {
            Serdata.appVersion = appVersion;
            Serdata.Serialization();
        }

        public void OnPageBarDraw()
        {

        }

        private void ApplySetting()
        {
            PlayerSettings.Android.useAPKExpansionFiles = Serdata.packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB;
            EditorUserBuildSettings.buildAppBundle = Serdata.packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE;

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, Serdata.scripting);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, Serdata.scripting);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, Serdata.scripting);

            // arm64
            var _googlePlaySupportARM64 = Serdata.scripting == ScriptingImplementation.IL2CPP && (Serdata.packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE || Serdata.packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB);
            if (_googlePlaySupportARM64)
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            }
            else
            {
                if (Serdata.supportARM64)
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
                else
                    PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
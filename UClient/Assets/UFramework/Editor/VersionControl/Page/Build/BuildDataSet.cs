/*
 * @Author: fasthro
 * @Date: 2020-10-15 12:26:08
 * @Description: 
 */
using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using UFramework.Assets;
using System.IO;
using UFramework.Editor.Preferences.Assets;

namespace UFramework.Editor.VersionControl
{
    public enum PACKAGE_TYPE
    {
        BASIC,
        GOOGLE_PLAY_APPBUNDLE,
        GOOGLE_PLAY_OBB,
    }

    public class VersionControl_BuildConfig : IConfigObject
    {
        public string name { get { return "VersionControl_BuildConfig"; } }
        public FileAddress address { get { return FileAddress.Editor; } }

        /// <summary>
        /// 应用版本
        /// </summary>
        public string appVersion;

        /// <summary>
        /// 包类型
        /// </summary>
        public PACKAGE_TYPE packageType;

        /// <summary>
        /// il2cpp
        /// </summary>
        public ScriptingImplementation scripting;

        /// <summary>
        /// 支持 arm64
        /// </summary>
        public bool supportARM64;

        /// <summary>
        /// version code
        /// </summary>
        public int versionCode;

        #region android

        public bool useCustomKeystore;
        public string keystoreName;
        public string keystorePass;
        public string keyaliasName;
        public string keyaliasPass;

        #endregion

        #region ios

        public string signingTeamID;

        #endregion

        public void Save()
        {
            UConfig.Write<VersionControl_BuildConfig>(this);
        }
    }

    [System.Serializable]
    public class BuildPageBuildTable
    {
        #region Serializable

        /// <summary>
        /// 包类型
        /// </summary>
        [LabelText("Type")]
        [OnValueChanged("OnValueChanged_PackageType")]
        [DisableIf("_isBuild")]
        public PACKAGE_TYPE packageType = PACKAGE_TYPE.BASIC;

        private void OnValueChanged_PackageType()
        {
            UConfig.Read<VersionControl_BuildConfig>().packageType = packageType;

            var app = UConfig.Read<AppConfig>();
            app.useAPKExpansionFiles = packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB;
            app.Save();

            PlayerSettings.Android.useAPKExpansionFiles = packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB;
            EditorUserBuildSettings.buildAppBundle = packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE;

            // google play arm64
            supportARM64 = scripting == ScriptingImplementation.IL2CPP && (packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE || packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// IL2CPP
        /// </summary>
        [LabelText("Scripting Backend")]
        [OnValueChanged("OnValueChanged_Scripting")]
        [DisableIf("_isBuild")]
        public ScriptingImplementation scripting = ScriptingImplementation.Mono2x;

        private void OnValueChanged_Scripting()
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, scripting);

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, scripting);
            // google play arm64
            supportARM64 = scripting == ScriptingImplementation.IL2CPP && (packageType == PACKAGE_TYPE.GOOGLE_PLAY_APPBUNDLE || packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB);

            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, scripting);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 是否支持ARM64
        /// </summary>
        [OnValueChanged("OnValueChanged_SupportARM64")]
        [DisableIf("_supportARM64")]
        [DisableIf("_isBuild")]
        public bool supportARM64;

#if UNITY_ANDROID
        private bool _supportARM64 = false;
#else
        private bool _supportARM64 = true;
#endif

        private void OnValueChanged_SupportARM64()
        {
            if (supportARM64)
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            else
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion

        private bool _isBuild;
        private bool _noBuild { get { return !_isBuild; } }
        public bool isBuild { get { return _isBuild; } }

        [HideIf("_noBuild")]
        [ProgressBar(0, 100, DrawValueLabel = false, ColorMember = "_progressColor")]
        public int progress;

        [HideIf("_noBuild")]
        [ProgressBar(0, 7, 0, 1, 0, Segmented = true, DrawValueLabel = false)]
        [LabelText("Build Progress")]
        public int totalProgress;

        private Color _progressColor(float value)
        {
            return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void BuildAssetBundle()
        {
            if (_isBuild) return;
            _BuildAssetBundle();
        }

        private void _BuildAssetBundle()
        {
            var page = new AssetBundlePage();
            page.OnRenderBefore();
            page.AnalysisAssets();
            page.BuildAssetsBundle(false);
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void BuildScripts()
        {
            if (_isBuild) return;
        }

        [ShowInInspector, Button]
        [DisableIf("_isBuild")]
        public void BuildApplication()
        {
            if (_isBuild) return;

            if (VersionPage.IsPublishVersion())
            {
                if (EditorUtility.DisplayDialog("Build", "The current version has been released. Do you want to rebuild it?", "Rebuild", "Cancel"))
                {
                    EditorCoroutineUtility.StartCoroutineOwnerless(BuildPlayer());
                }
            }
            else EditorCoroutineUtility.StartCoroutineOwnerless(BuildPlayer());
        }

        IEnumerator BuildPlayer()
        {
            _isBuild = true;
            totalProgress = 0;
            progress = 0;

            // bundle
            var manifestPath = IOPath.PathCombine(App.UTempDirectory, App.BundlePlatformName, AssetManifest.AssetBundleName + App.AssetBundleExtension);
            if (!IOPath.FileExists(manifestPath))
                _BuildAssetBundle();
            yield return new EditorWaitForSeconds(1);
            totalProgress = 1;

            // copy res
            progress = 0;
            IOPath.DirectoryClear(IOPath.PathCombine(Application.streamingAssetsPath, App.BundlePlatformName));
            string[] files = IOPath.DirectoryGetFiles(IOPath.PathCombine(App.UTempDirectory, App.BundlePlatformName), "*" + App.AssetBundleExtension, SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var fp = files[i];
                var fn = IOPath.FileName(fp, true);
                var dest = IOPath.PathCombine(Application.streamingAssetsPath, App.BundlePlatformName, fn);
                IOPath.FileCopy(files[i], dest);
                yield return null;
                progress = (int)((float)(i + 1) / (float)files.Length * 100f);
            }
            yield return new EditorWaitForSeconds(1);
            totalProgress = 2;

            // version
            progress = 0;
            VersionPage.BuildVersion();
            yield return null;
            IOPath.FileCopy(IOPath.PathCombine(App.UTempDirectory, App.VersionFileName), IOPath.PathCombine(Application.streamingAssetsPath, App.VersionFileName));
            yield return new EditorWaitForSeconds(1);
            totalProgress = 3;

            // setting
            progress = 0;
            var build = UConfig.Read<VersionControl_BuildConfig>();
            build.versionCode++;
            build.Save();

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode = build.versionCode;
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = build.versionCode.ToString();
                    break;
            }
            PlayerSettings.bundleVersion = build.appVersion;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            yield return new EditorWaitForSeconds(1);
            totalProgress = 4;

            // build
            progress = 0;
            var outPath = IOPath.PathCombine(Environment.CurrentDirectory, "Build", GetBuildPlatformName(EditorUserBuildSettings.activeBuildTarget));
            var targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName != null)
            {
                var scenes = EditorBuildSettings.scenes.Where(x => x.enabled).ToArray();
                var options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
                var locationPathName = IOPath.PathCombine(outPath, targetName);

                BuildPipeline.BuildPlayer(scenes, locationPathName, EditorUserBuildSettings.activeBuildTarget, options);
            }
            totalProgress = 5;
            yield return new EditorWaitForSeconds(1);

            // save version
            progress = 0;
            VersionPage.BuildPublishFileRecords();
            yield return new EditorWaitForSeconds(1);
            totalProgress = 6;

            // document
            var ver = UConfig.Read<VersionControl_VersionConfig>();
            var documentPath = IOPath.PathCombine(Environment.CurrentDirectory, "Build", GetBuildPlatformName(EditorUserBuildSettings.activeBuildTarget), "app-version-" + ver.version);
            IOPath.DirectoryClear(documentPath);
            IOPath.FileCopy(IOPath.PathCombine(App.UTempDirectory, App.VersionFileName), IOPath.PathCombine(documentPath, App.VersionFileName));
            yield return new EditorWaitForSeconds(1);
            totalProgress = 7;

            yield return new EditorWaitForSeconds(1);
            Debug.Log("build application finished! [" + targetName + "]");
            _isBuild = false;
            EditorUtility.RevealInFinder(outPath);
        }

        /// <summary>
        /// 构建目标包文件名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        static string GetBuildTargetName(BuildTarget target)
        {
            var time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var name = "app-v" + PlayerSettings.bundleVersion + ".";
            var version = UConfig.Read<VersionControl_VersionConfig>().version;
            switch (target)
            {
                case BuildTarget.Android:
                    return string.Format("{0}{1}-{2}.apk", name, version, time);

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return string.Format("{0}{1}-{2}.exe", name, version, time);

                case BuildTarget.StandaloneOSX:
                    return name + ".app";

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log(target.ToString() + " not implemented.");
                    return null;
            }
        }

        /// <summary>
        /// 构建平台名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        static string GetBuildPlatformName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
                case BuildTarget.StandaloneOSX:
                    return "OSX";
                default:
                    return null;
            }
        }
    }

    [System.Serializable]
    public class BuildPagePatchTable
    {
        public int version;

        [ShowInInspector, Button, HorizontalGroup]
        public void BuildPatch()
        {

        }
    }

    [System.Serializable]
    public class BuildSettingPageAndroidTable
    {
        [BoxGroup("KeyStore")]
        [OnValueChanged("OnValueChanged_useCustomKeystore")]
        public bool useCustomKeystore;

        private void OnValueChanged_useCustomKeystore()
        {
            PlayerSettings.Android.useCustomKeystore = useCustomKeystore;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [HideInInspector]
        private bool _useCustomKeystore { get { return !useCustomKeystore; } }

        [DisableIf("_useCustomKeystore")]
        [FilePath(Extensions = ".keystore")]
        [BoxGroup("KeyStore")]
        [OnValueChanged("OnValueChanged_keystoreName")]
        public string keystoreName;

        private void OnValueChanged_keystoreName()
        {
            PlayerSettings.Android.keystoreName = keystoreName;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [DisableIf("_useCustomKeystore")]
        [BoxGroup("KeyStore")]
        [OnValueChanged("OnValueChanged_keystorePass")]
        public string keystorePass;

        private void OnValueChanged_keystorePass()
        {
            PlayerSettings.Android.keystorePass = keystorePass;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [DisableIf("_useCustomKeystore")]
        [BoxGroup("KeyStore")]
        [OnValueChanged("OnValueChanged_keyaliasName")]
        public string keyaliasName;

        private void OnValueChanged_keyaliasName()
        {
            PlayerSettings.Android.keyaliasName = keyaliasName;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [DisableIf("_useCustomKeystore")]
        [BoxGroup("KeyStore")]
        [OnValueChanged("OnValueChanged_keyaliasPass")]
        public string keyaliasPass;

        private void OnValueChanged_keyaliasPass()
        {
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [System.Serializable]
    public class BuildSettingPageIOSTable
    {
        [BoxGroup("Signing")]
        public string signingTeamID;
    }
}
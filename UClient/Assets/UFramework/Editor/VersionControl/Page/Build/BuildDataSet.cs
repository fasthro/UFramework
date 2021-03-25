// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-10-15 12:26:08
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UFramework.Core;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;
using System.IO;
using UFramework.Editor.Preferences.AssetBundle;
using System.Collections.Generic;
using UFramework.Editor.Preferences.BuildFiles;
using UFramework.Editor.Preferences.Lua;
using UFramework.Editor.VersionControl.Version;

namespace UFramework.Editor.VersionControl.Build
{
    public enum PACKAGE_TYPE
    {
        BASIC,
        GOOGLE_PLAY_APPBUNDLE,
        GOOGLE_PLAY_OBB,
    }

    public class VersionControl_Build_Config : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Editor;

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

        public void Serialize()
        {
            Serializer<VersionControl_Build_Config>.Serialize(this);
        }
    }

    [System.Serializable]
    public class BuildPageBuildTable
    {
        #region Serializable

        /// <summary>
        /// 包类型
        /// </summary>
        [LabelText("Type")] [OnValueChanged("OnValueChanged_PackageType")] [DisableIf("_isBuild")]
        public PACKAGE_TYPE packageType = PACKAGE_TYPE.BASIC;

        private void OnValueChanged_PackageType()
        {
            Serializer<VersionControl_Build_Config>.Instance.packageType = packageType;
            Serializer<VersionControl_Build_Config>.Instance.Serialize();

            Core.Serializer<AppConfig>.Instance.useAPKExpansionFiles = packageType == PACKAGE_TYPE.GOOGLE_PLAY_OBB;
            Core.Serializer<AppConfig>.Instance.Serialize();

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
        [LabelText("Scripting Backend")] [OnValueChanged("OnValueChanged_Scripting")] [DisableIf("_isBuild")]
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
        [OnValueChanged("OnValueChanged_SupportARM64")] [DisableIf("_supportARM64")] [DisableIf("_isBuild")]
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

#if UNITY_EDITOR_WIN || Une

        private bool _isFullScreen => fullScreenMode != FullScreenMode.FullScreenWindow;
        [OnValueChanged("OnValueChanged_Win")] public FullScreenMode fullScreenMode = FullScreenMode.FullScreenWindow;

        [OnValueChanged("OnValueChanged_Win")] [ShowIf("_isFullScreen")]
        public int resolutionX = 1920;

        [OnValueChanged("OnValueChanged_Win")] [ShowIf("_isFullScreen")]
        public int resolutionY = 1080;

        private void OnValueChanged_Win()
        {
            var ac = Serializer<AppConfig>.Instance;
            ac.fullScreenMode = fullScreenMode;
            ac.resolutionX = resolutionX;
            ac.resolutionY = resolutionY;
            ac.Serialize();
        }
#endif

        #endregion

        private bool _isBuild;

        private bool _noBuild => !_isBuild;

        public bool isBuild => _isBuild;

        [HideIf("_noBuild")] [ProgressBar(0, 100, DrawValueLabel = false, ColorMember = "_progressColor")]
        public int progress;

        [HideIf("_noBuild")] [ProgressBar(0, 10, 0, 1, 0, Segmented = true, DrawValueLabel = false)] [LabelText("Build Progress")]
        public int totalProgress;

        private Color _progressColor(float value)
        {
            return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void AnalysisAssetBundle()
        {
            if (_isBuild) return;
            _AnalysisAssetBundle();
        }

        private void _AnalysisAssetBundle()
        {
            var page = new AssetBundlePage();
            page.OnRenderBefore();
            page.AnalysisAssets();
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
            page.BuildAssetsBundle(false);
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void CopyAssetBundle()
        {
            if (_isBuild) return;
            EditorCoroutineUtility.StartCoroutineOwnerless(_CopyAssetBundle());
        }

        private IEnumerator _CopyAssetBundle()
        {
            progress = 0;
            var assetBundlePath = IOPath.PathCombine(UApplication.TempDirectory, Platform.BuildTargetCurrentName);
            var assetBundleDataPath = IOPath.PathCombine(Application.streamingAssetsPath, Platform.BuildTargetCurrentName);
            IOPath.DirectoryClear(assetBundleDataPath);
            var files = IOPath.DirectoryGetFiles(assetBundlePath, "*" + Assets.Extension, SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; i++)
            {
                var fp = files[i];
                var fn = IOPath.FileName(fp, true);
                var dest = IOPath.PathCombine(assetBundleDataPath, fn);
                IOPath.FileCopy(files[i], dest);
                yield return null;
                progress = (int) ((float) (i + 1) / (float) files.Length * 100f);
            }

            AssetDatabase.Refresh();
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void CopyVersion()
        {
            if (_isBuild) return;
            _CopyVersion();
        }

        private void _CopyVersion()
        {
            var versionFilePath = IOPath.PathCombine(UApplication.TempDirectory, Core.Version.FileName);
            IOPath.FileCopy(versionFilePath, IOPath.PathCombine(Application.streamingAssetsPath, Core.Version.FileName));
            AssetDatabase.Refresh();
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void BuildScripts()
        {
            if (_isBuild) return;
            _BuildScripts();
        }

        private void _BuildScripts()
        {
            var page = new LuaPage();
            page.OnRenderBefore();
            page.BuildScripts(Core.Serializer<LuaConfig>.Instance.byteEncode, false);
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void CopyScripts()
        {
            if (_isBuild) return;
            EditorCoroutineUtility.StartCoroutineOwnerless(_CopyScripts());
        }

        IEnumerator _CopyScripts()
        {
            progress = 0;
            var luaPath = IOPath.PathCombine(UApplication.TempDirectory, "Lua");
            var luaDataPath = IOPath.PathCombine(Application.streamingAssetsPath, "Lua");
            IOPath.DirectoryClear(luaDataPath);
            var files = IOPath.DirectoryGetFiles(luaPath, "*.lua", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; i++)
            {
                var fp = files[i];
                var fn = IOPath.FileName(fp, true);
                var dn = IOPath.PathUnitySeparator(Path.GetDirectoryName(fp)).Replace(luaPath, "").TrimStart('/').TrimStart('\\').TrimEnd('/').TrimEnd('\\');
                var dest = IOPath.PathCombine(luaDataPath, dn, fn);
                IOPath.FileCopy(files[i], dest);
                yield return null;
                progress = (int) ((float) (i + 1) / (float) files.Length * 100f);
            }

            AssetDatabase.Refresh();
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void BuildFiles()
        {
            if (_isBuild) return;
            _BuildFiles();
        }

        private void _BuildFiles()
        {
            var page = new BuildFilesPage();
            page.OnRenderBefore();
            page.Build(true);
        }

        [ShowInInspector, Button, HorizontalGroup]
        [DisableIf("_isBuild")]
        public void CopyFiles()
        {
            if (_isBuild) return;
            EditorCoroutineUtility.StartCoroutineOwnerless(_CopyFiles());
        }

        IEnumerator _CopyFiles()
        {
            progress = 0;
            var filePath = IOPath.PathCombine(UApplication.TempDirectory, "Files");
            var fileDataPath = IOPath.PathCombine(Application.streamingAssetsPath, "Files");
            IOPath.DirectoryClear(fileDataPath);
            var files = IOPath.DirectoryGetFiles(filePath, "*.*", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; i++)
            {
                var fp = files[i];
                var fe = IOPath.FileExtensionName(fp);

                if (fe.Equals(".meta"))
                    continue;

                var fn = IOPath.FileName(fp, true);
                var dn = IOPath.PathUnitySeparator(Path.GetDirectoryName(fp)).Replace(filePath, "").TrimStart('/').TrimStart('\\').TrimEnd('/').TrimEnd('\\');
                var dest = IOPath.PathCombine(fileDataPath, dn, fn);
                IOPath.FileCopy(files[i], dest);
                yield return null;
                progress = (int) ((float) (i + 1) / (float) files.Length * 100f);
            }

            AssetDatabase.Refresh();
        }

        #region build application

        [ShowInInspector, Button, HorizontalGroup("build")]
        [DisableIf("_isBuild")]
        public void BuildApplication()
        {
            _BuildApplication(false);
        }

        [ShowInInspector, Button, HorizontalGroup("build")]
        [DisableIf("_isBuild")]
        public void BuildApplicationWithAll()
        {
            _BuildApplication(true);
        }

        private void _BuildApplication(bool isBuildAll)
        {
            if (_isBuild) return;

            if (Serializer<AppConfig>.Instance.isDevelopmentVersion)
            {
                EditorUtility.DisplayDialog("Build", "当前为开发环境, 无法构建应用. 请切换到 Version Contorl -> Application 页进行环境切换.", "确定");
                return;
            }

            if (VersionPage.IsPublishVersion())
            {
                if (EditorUtility.DisplayDialog("Build", "当前版本已经存在，是否重新构建版本?", "重新构建", "取消"))
                {
                    EditorCoroutineUtility.StartCoroutineOwnerless(_BuildApplicationAsync(isBuildAll));
                }
            }
            else EditorCoroutineUtility.StartCoroutineOwnerless(_BuildApplicationAsync(isBuildAll));
        }

        IEnumerator _BuildApplicationAsync(bool isBuildAll)
        {
            _isBuild = true;
            totalProgress = 0;
            progress = 0;

            var assetBundlePath = IOPath.PathCombine(UApplication.TempDirectory, Platform.BuildTargetCurrentName);
            var versionFilePath = IOPath.PathCombine(UApplication.TempDirectory, Core.Version.FileName);
            var outPath = IOPath.PathCombine(UApplication.BuildDirectory, Platform.BuildTargetCurrentName);

            // bundle
            totalProgress++;
            yield return new EditorWaitForSeconds(1);

            if (isBuildAll) _BuildAssetBundle();
            yield return new EditorWaitForSeconds(1);

            // copy bundle
            if (isBuildAll)
                yield return _CopyAssetBundle();

            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // lua script
            if (isBuildAll) _BuildScripts();
            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // copy lua script
            if (isBuildAll)
                yield return _CopyScripts();

            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // build files
            if (isBuildAll) _BuildFiles();
            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // copy build files
            if (isBuildAll)
                yield return _CopyFiles();

            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // version
            progress = 0;
            VersionPage.BuildVersion(IOPath.PathCombine(UApplication.TempDirectory, Core.Version.FileName));
            _CopyVersion();
            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // setting
            progress = 0;
            var buildSerdata = Serializer<VersionControl_Build_Config>.Instance;
            buildSerdata.versionCode++;
            buildSerdata.Serialize();

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode = buildSerdata.versionCode;
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = buildSerdata.versionCode.ToString();
                    break;
            }

            PlayerSettings.bundleVersion = buildSerdata.appVersion;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // build player
            progress = 0;
            var targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName != null)
            {
                var scenes = EditorBuildSettings.scenes.Where(x => x.enabled).ToArray();
                var options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
                var locationPathName = IOPath.PathCombine(outPath, targetName);

                BuildPipeline.BuildPlayer(scenes, locationPathName, EditorUserBuildSettings.activeBuildTarget, options);
            }

            totalProgress++;
            yield return new EditorWaitForSeconds(1);

            // 保存发布版本记录
            progress = 0;
            VersionPage.BuildReleaseRecords();
            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            // 构建版本目录
            var dn = "data-v" + Core.Serializer<VersionControl_Version_Config>.Instance.version;
            var documentPath = IOPath.PathCombine(outPath, dn);
            IOPath.DirectoryClear(documentPath);
            IOPath.FileCopy(versionFilePath, IOPath.PathCombine(documentPath, Core.Version.FileName));
            yield return new EditorWaitForSeconds(1);
            totalProgress++;

            yield return new EditorWaitForSeconds(1);
            Logger.Debug("build application finished! [" + targetName + "]");
            _isBuild = false;
            EditorUtility.RevealInFinder(outPath);
        }

        /// <summary>
        /// 构建补丁文件
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        static string GetBuildTargetName(BuildTarget target)
        {
            var time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var name = "app-" + PlayerSettings.bundleVersion + "-v";
            var version = Core.Serializer<VersionControl_Version_Config>.Instance.version;
            switch (target)
            {
                case BuildTarget.Android:
                    return $"{name}{version}-{time}.apk";

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return $"{name}{version}-{time}.exe";

                case BuildTarget.StandaloneOSX:
                    return name + ".app";

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Logger.Debug(target.ToString() + " not implemented.");
                    return null;
            }
        }

        #endregion

        #region build patch

        [ShowInInspector, Button, HorizontalGroup("build")]
        [DisableIf("_isBuild")]
        public void BuildPatch()
        {
            if (_isBuild) return;
            _BuildPatch();
        }

        void _BuildPatch()
        {
            _isBuild = true;
            totalProgress = 0;
            progress = 0;

            var versionConfig = Core.Serializer<VersionControl_Version_Config>.Instance;
            var version = versionConfig.GetPlatformVersion();

            if (!version.HasNewPatchVersionWaitBuild())
            {
                _isBuild = false;
                EditorUtility.DisplayDialog("Patch", "请前往 Version Contorl -> Version 页创建补丁版本.", "确定");
                return;
            }

            var dataPath = IOPath.PathCombine(UApplication.BuildDirectory, Platform.BuildTargetCurrentName, "data-v" + versionConfig.version);

            var nPatchVersionCode = version.GetNextPatchVersion();
            var oPatchVersionCode = nPatchVersionCode - 1;
            var currentPatch = version.GetPatchVersion(oPatchVersionCode);

            string oVersionPath = null;
            if (currentPatch != null)
                oVersionPath = IOPath.PathCombine(dataPath, currentPatch.displayName, Core.Version.FileName);
            else oVersionPath = IOPath.PathCombine(dataPath, Core.Version.FileName);

            if (!IOPath.FileExists(oVersionPath))
            {
                EditorUtility.DisplayDialog("Patch", "当前版本应用不存在, 无法构建版本补丁.", "确定");
                _isBuild = false;
                return;
            }

            // 查询需要更新的文件
            // files
            var oVer = Core.Version.LoadVersion(oVersionPath);
            var fileMap = new Dictionary<string, VFile>();
            for (var i = 0; i < oVer.files.Length; i++)
                fileMap.Add(oVer.files[i].name, oVer.files[i]);

            var nPatchFiles = new List<VFile>();
            VersionPage.CheckVersionFiles(out var newFiles);
            for (var i = 0; i < newFiles.Length; i++)
            {
                var file = newFiles[i];
                if (!fileMap.ContainsKey(file.name))
                    nPatchFiles.Add(file);
                else
                {
                    if (!file.hash.Equals(fileMap[file.name].hash))
                        nPatchFiles.Add(file);
                }
            }

            // sfiles
            var sFileMap = new Dictionary<string, VScriptFile>();
            for (var i = 0; i < oVer.sFiles.Length; i++)
            {
                var index = $"{oVer.sFiles[i].dirIndex}-{oVer.sFiles[i].name}";
                sFileMap.Add(index, oVer.sFiles[i]);
            }

            var nPatchSFiles = new List<VScriptFile>();
            VersionPage.CheckVersionScripts(out var newSDirs, out var newSFiles);
            for (var i = 0; i < newSFiles.Length; i++)
            {
                var file = newSFiles[i];
                var index = $"{file.dirIndex}-{file.name}";
                if (!sFileMap.ContainsKey(index))
                    nPatchSFiles.Add(file);
                else
                {
                    if (!file.hash.Equals(sFileMap[index].hash))
                        nPatchSFiles.Add(file);
                }
            }

            // bfiles
            var bFileMap = new Dictionary<string, VBuildFile>();
            for (var i = 0; i < oVer.bFiles.Length; i++)
            {
                var index = $"{oVer.bFiles[i].dirIndex}-{oVer.bFiles[i].name}";
                bFileMap.Add(index, oVer.bFiles[i]);
            }

            var nPatchBFiles = new List<VBuildFile>();
            VersionPage.CheckVersionBuildFiles(out var newBDirs, out var newBFiles);
            for (var i = 0; i < newBFiles.Length; i++)
            {
                var file = newBFiles[i];
                var index = $"{file.dirIndex}-{file.name}";
                if (!bFileMap.ContainsKey(index))
                    nPatchBFiles.Add(file);
                else
                {
                    if (!file.hash.Equals(bFileMap[index].hash))
                        nPatchBFiles.Add(file);
                }
            }

            if (nPatchFiles.Count > 0 || nPatchSFiles.Count > 0 || nPatchBFiles.Count > 0)
            {
                foreach (var file in nPatchFiles)
                    Logger.Debug(">> patch file: " + file.name);

                foreach (var file in nPatchSFiles)
                    Logger.Debug(">> patch script file: " + file.name);

                foreach (var file in nPatchBFiles)
                    Logger.Debug(">> patch build file: " + file.name);
            }
            else
            {
                EditorUtility.DisplayDialog("Patch", "资源和脚本已经是最新版本, 无需构建补. 请尝试 Version Contorl -> Build AssetBundle & Build Script 之后在尝试构建.", "确定");
                _isBuild = false;
                return;
            }

            // 更新版本内容
            var newPatch = new VEditorPatch();
            newPatch.files.AddRange(nPatchFiles);
            newPatch.sFiles.AddRange(nPatchSFiles);
            newPatch.bFiles.AddRange(nPatchBFiles);
            newPatch.aVersion = versionConfig.version;
            newPatch.pVersion = nPatchVersionCode;
            newPatch = version.UpdatePatch(newPatch);


            // 生成补丁文件
            var nPathDir = IOPath.PathCombine(dataPath, newPatch.displayName);
            IOPath.DirectoryClear(nPathDir);

            var nPatchPath = IOPath.PathCombine(dataPath, "patch");
            IOPath.DirectoryClear(nPatchPath);

            var fc = nPatchFiles.Count + nPatchSFiles.Count + nPatchBFiles.Count;
            string[] _files = new string[fc];
            string[] _parents = new string[fc];
            int _index = 0;
            foreach (var item in nPatchFiles)
            {
                var source = IOPath.PathCombine(UApplication.TempDirectory, Platform.BuildTargetCurrentName, item.name);
                var dest = IOPath.PathCombine(nPatchPath, item.name);
                IOPath.FileCopy(source, dest);

                _files[_index] = dest;
                _parents[_index] = Platform.BuildTargetCurrentName;
                _index++;
            }

            foreach (var item in nPatchSFiles)
            {
                var tp = IOPath.PathCombine("Lua", newSDirs[item.dirIndex], item.name);
                var source = IOPath.PathCombine(UApplication.TempDirectory, tp);
                var dest = IOPath.PathCombine(nPatchPath, tp);
                IOPath.FileCopy(source, dest);

                _files[_index] = dest;
                _parents[_index] = IOPath.PathCombine("Lua", newSDirs[item.dirIndex]);
                _index++;
            }

            foreach (var item in nPatchBFiles)
            {
                var tp = IOPath.PathCombine("Files", newBDirs[item.dirIndex], item.name);
                var source = IOPath.PathCombine(UApplication.TempDirectory, tp);
                var dest = IOPath.PathCombine(nPatchPath, tp);
                IOPath.FileCopy(source, dest);

                _files[_index] = dest;
                _parents[_index] = IOPath.PathCombine("Files", newBDirs[item.dirIndex]);
                _index++;
            }

            var zipPath = IOPath.PathCombine(nPathDir, $"{newPatch.displayName}.zip");
            UZip.Zip(_files, _parents, zipPath, null, null);

            // 生成版本信息文件
            newPatch.len = IOPath.FileSize(zipPath);
            newPatch = version.UpdatePatch(newPatch);
            versionConfig.Serialize();
            VersionPage.BuildVersion(IOPath.PathCombine(nPathDir, Core.Version.FileName));

            _isBuild = false;
            EditorUtility.RevealInFinder(nPatchPath);
            Logger.Debug("build patch finished! [" + newPatch.displayName + "]");
        }

        #endregion
    }

    [System.Serializable]
    public class BuildSettingPageAndroidTable
    {
        [BoxGroup("KeyStore")] [OnValueChanged("OnValueChanged_useCustomKeystore")]
        public bool useCustomKeystore;

        private void OnValueChanged_useCustomKeystore()
        {
            PlayerSettings.Android.useCustomKeystore = useCustomKeystore;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [HideInInspector] private bool _useCustomKeystore => !useCustomKeystore;

        [DisableIf("_useCustomKeystore")] [FilePath(Extensions = ".keystore")] [BoxGroup("KeyStore")] [OnValueChanged("OnValueChanged_keystoreName")]
        public string keystoreName;

        private void OnValueChanged_keystoreName()
        {
            PlayerSettings.Android.keystoreName = keystoreName;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [DisableIf("_useCustomKeystore")] [BoxGroup("KeyStore")] [OnValueChanged("OnValueChanged_keystorePass")]
        public string keystorePass;

        private void OnValueChanged_keystorePass()
        {
            PlayerSettings.Android.keystorePass = keystorePass;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [DisableIf("_useCustomKeystore")] [BoxGroup("KeyStore")] [OnValueChanged("OnValueChanged_keyaliasName")]
        public string keyaliasName;

        private void OnValueChanged_keyaliasName()
        {
            PlayerSettings.Android.keyaliasName = keyaliasName;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [DisableIf("_useCustomKeystore")] [BoxGroup("KeyStore")] [OnValueChanged("OnValueChanged_keyaliasPass")]
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
        [BoxGroup("Signing")] public string signingTeamID;
    }
}
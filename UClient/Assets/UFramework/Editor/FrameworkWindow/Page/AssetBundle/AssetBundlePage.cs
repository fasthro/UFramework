/*
 * @Author: fasthro
 * @Date: 2020-06-29 17:00:30
 * @Description: AssetBundle
 */
using System.Collections.Generic;
using System.IO;
using AssetBundleBrowser.AssetBundleDataSource;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UFramework.ResLoader;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace UFramework.FrameworkWindow
{
    public class AssetBundlePage : IPage
    {
        public string menuName { get { return "AssetBundle"; } }
        static AssetBundleAssetItemConfig describeObject;

        private HashSet<string> assetRecorder = new HashSet<string>();

        [ShowInInspector]
        [TabGroup("Assets")]
        public List<AssetBundleAssetItem> assets = new List<AssetBundleAssetItem>();

        [ShowInInspector]
        [TabGroup("Dependencies Assets")]
        public List<AssetBundleAssetItem> dependencieAssets = new List<AssetBundleAssetItem>();

        public object GetInstance()
        {
            return this;
        }
        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AssetBundleAssetItemConfig>();
            assets = describeObject.assets;
            Refresh();
        }

        public void OnDrawFunctoinButton()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
            {
                Refresh();
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build All")))
            {
                BuildAll();
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clear Build All")))
            {
                ClearuildAll();
            }
        }

        public void OnApply()
        {
            describeObject.assets = assets;
            describeObject.Save();
        }

        private void Refresh()
        {
            // Parse PathItemConfig
            assets.Clear();
            assetRecorder.Clear();
            var pathConfig = UConfig.Read<AssetBundleAssetPathItemConfig>();
            for (int i = 0; i < pathConfig.assetPathItems.Count; i++)
            {
                ParsePathItem(pathConfig.assetPathItems[i]);
            }

            // Parse Dependencies
            dependencieAssets.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                ParseDependencies(assets[i]);
            }

            // gen config relation
            var resAssetInfoConfig = UConfig.Read<ResLoaderConfig>();
            resAssetInfoConfig.assetInfoDictionary.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                resAssetInfoConfig.assetInfoDictionary.Add(assets[i].path, GenResAssetInfo(assets[i]));
            }
            resAssetInfoConfig.Save();
        }

        /// <summary>
        /// Parse Path Item
        /// </summary>
        /// <param name="pathItem"></param>
        private void ParsePathItem(AssetBundleAssetPathItem pathItem)
        {
            // File
            if (pathItem.buildType == AssetBundleBuildPathType.File)
            {
                var assetItem = new AssetBundleAssetItem();
                assetItem.path = pathItem.path;
                assetItem.assetBundleName = FormatAssetBundleName(pathItem.path);
                assetItem.assetType = pathItem.assetType;

                AddAssetItem(assetItem);
            }
            // DirectoryFile
            else if (pathItem.buildType == AssetBundleBuildPathType.DirectoryFile)
            {
                var files = Directory.GetFiles(pathItem.path, pathItem.pattern, SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    if (file.EndsWith(".meta")) continue;
                    var assetItem = new AssetBundleAssetItem();
                    assetItem.path = file;
                    assetItem.assetBundleName = FormatAssetBundleName(file);
                    assetItem.assetType = pathItem.assetType;

                    AddAssetItem(assetItem);
                }
            }
            // Directory
            else if (pathItem.buildType == AssetBundleBuildPathType.Directory)
            {
                var files = Directory.GetFiles(pathItem.path, pathItem.pattern, SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    if (file.EndsWith(".meta")) continue;
                    var assetItem = new AssetBundleAssetItem();
                    assetItem.path = file;
                    assetItem.assetBundleName = FormatAssetBundleName(pathItem.path);
                    assetItem.assetType = pathItem.assetType;

                    AddAssetItem(assetItem);
                }
            }
            // SubDirectory
            else if (pathItem.buildType == AssetBundleBuildPathType.SubDirectory)
            {
                var directorys = Directory.GetDirectories(pathItem.path);
                for (int d = 0; d < directorys.Length; d++)
                {
                    var directorie = directorys[d];
                    var files = Directory.GetFiles(directorie, pathItem.pattern, SearchOption.AllDirectories);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var file = files[i];
                        if (file.EndsWith(".meta")) continue;
                        var assetItem = new AssetBundleAssetItem();
                        assetItem.path = file;
                        assetItem.assetBundleName = FormatAssetBundleName(directorie);
                        assetItem.assetType = pathItem.assetType;

                        AddAssetItem(assetItem);
                    }
                }
            }
            // Standard
            else if (pathItem.buildType == AssetBundleBuildPathType.Standard)
            {
                // 查询当前目录
                string customDirectory = GetStandardDirectory(pathItem.path, pathItem);
                if (IOPath.DirectoryExists(customDirectory))
                {
                    var files = Directory.GetFiles(customDirectory, pathItem.pattern, SearchOption.AllDirectories);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var file = files[i];
                        if (file.EndsWith(".meta")) continue;
                        var assetItem = new AssetBundleAssetItem();
                        assetItem.path = file;
                        assetItem.assetBundleName = FormatAssetBundleName(customDirectory);
                        assetItem.assetType = pathItem.assetType;

                        AddAssetItem(assetItem);
                    }
                }
                // 查询子文件夹是否符合Standard格式
                var directorys = Directory.GetDirectories(pathItem.path, "*.*", SearchOption.TopDirectoryOnly);
                for (int d = 0; d < directorys.Length; d++)
                {
                    var directory = directorys[d];
                    customDirectory = GetStandardDirectory(directory, pathItem);
                    if (IOPath.DirectoryExists(customDirectory))
                    {
                        var files = Directory.GetFiles(customDirectory, pathItem.pattern, SearchOption.AllDirectories);
                        for (int i = 0; i < files.Length; i++)
                        {
                            var file = files[i];
                            if (file.EndsWith(".meta")) continue;
                            var assetItem = new AssetBundleAssetItem();
                            assetItem.path = file;
                            assetItem.assetBundleName = FormatAssetBundleName(customDirectory);
                            assetItem.assetType = pathItem.assetType;

                            AddAssetItem(assetItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parse Asset Dependencies
        /// </summary>
        /// <param name="assetItem"></param>
        private void ParseDependencies(AssetBundleAssetItem assetItem)
        {
            var dependencies = AssetDatabase.GetDependencies(assetItem.path);
            for (int i = 0; i < dependencies.Length; i++)
            {
                var dependencie = dependencies[i];
                if (!dependencie.Equals(assetItem.path))
                {
                    var item = new AssetBundleAssetItem();
                    item.path = dependencie;
                    item.assetBundleName = FormatAssetBundleName(dependencie);
                    item.assetType = ParseAssetType(dependencie);
                    AddAssetItem(item, true);
                }
            }
        }

        /// <summary>
        /// Get Standard Directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="pathItem"></param>
        /// <returns></returns>
        private string GetStandardDirectory(string directory, AssetBundleAssetPathItem pathItem)
        {
            string customDirectory = "";
            if (pathItem.assetType == AssetBundleBuildAssetType.Prefab)
            {
                customDirectory = IOPath.PathCombine(directory, "Prefab");
            }
            else if (pathItem.assetType == AssetBundleBuildAssetType.Texture)
            {
                customDirectory = IOPath.PathCombine(directory, "Texture");
            }
            else if (pathItem.assetType == AssetBundleBuildAssetType.Materail)
            {
                customDirectory = IOPath.PathCombine(directory, "Materail");
            }
            else if (pathItem.assetType == AssetBundleBuildAssetType.Animation)
            {
                customDirectory = IOPath.PathCombine(directory, "Animation");
            }
            else if (pathItem.assetType == AssetBundleBuildAssetType.AnimatorController)
            {
                customDirectory = IOPath.PathCombine(directory, "AnimatorController");
            }
            else if (pathItem.assetType == AssetBundleBuildAssetType.AnimatorController)
            {
                customDirectory = IOPath.PathCombine(directory, "AnimatorController");
            }
            return customDirectory;
        }

        /// <summary>
        /// 格式化 AssetBundleName
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private string FormatAssetBundleName(string bundleName)
        {
            bundleName = IOPath.PathUnitySeparator(bundleName);
            string ext = Path.GetExtension(bundleName);
            if (!string.IsNullOrEmpty(ext))
            {
                bundleName = bundleName.Substring(0, bundleName.Length - ext.Length);
            }
            string dpName = "Assets/";
            if (bundleName.StartsWith("Assets/"))
            {
                return bundleName.Substring(dpName.Length);
            }
            return bundleName;
        }

        /// <summary>
        /// 分析资源类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private AssetBundleBuildAssetType ParseAssetType(string path)
        {
            var suffix = Path.GetExtension(path);
            if (suffix.Equals(".prefab"))
            {
                return AssetBundleBuildAssetType.Prefab;
            }
            else if (suffix.Equals(".mat"))
            {
                return AssetBundleBuildAssetType.Materail;
            }
            else if (suffix.Equals(".png"))
            {
                return AssetBundleBuildAssetType.Texture;
            }
            else if (suffix.Equals(".anim"))
            {
                return AssetBundleBuildAssetType.Animation;
            }
            else if (suffix.Equals(".controller"))
            {
                return AssetBundleBuildAssetType.AnimatorController;
            }
            return AssetBundleBuildAssetType.File;
        }

        /// <summary>
        /// Add Asset Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isDependencie"></param>
        private void AddAssetItem(AssetBundleAssetItem item, bool isDependencie = false)
        {
            item.path = IOPath.PathUnitySeparator(item.path);
            item.size = IOPath.FileSize(item.path);

            if (!assetRecorder.Contains(item.path))
            {
                assetRecorder.Add(item.path);
                if (!isDependencie)
                {
                    assets.Add(item);
                }
                else
                {
                    dependencieAssets.Add(item);
                }
                SetAssetBundleName(item);
            }
        }

        /// <summary>
        /// 设置 AssetBundle Name
        /// </summary>
        /// <param name="assetItem"></param>
        private void SetAssetBundleName(AssetBundleAssetItem assetItem)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetItem.path);
            assetImporter.assetBundleName = assetItem.assetBundleName;
        }

        /// <summary>
        /// 生成 ResAssetInfo
        /// </summary>
        /// <param name="assetItem"></param>
        /// <returns></returns>
        private ResAssetInfo GenResAssetInfo(AssetBundleAssetItem assetItem)
        {
            var resInfo = new ResAssetInfo();
            resInfo.assetBundleName = assetItem.assetBundleName;
            resInfo.size = assetItem.size;
            resInfo.md5 = IOPath.FileMD5(assetItem.path);
            return resInfo;
        }

        /// <summary>
        /// Buid All Bundle
        /// </summary>
        private void BuildAll()
        {
            if (!IOPath.DirectoryExists(App.BundleDirectory))
            {
                IOPath.DirectoryClear(App.BundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(App.BundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private void ClearuildAll()
        {
            IOPath.DirectoryClear(App.BundleDirectory);
            BuildAll();
        }
    }
}
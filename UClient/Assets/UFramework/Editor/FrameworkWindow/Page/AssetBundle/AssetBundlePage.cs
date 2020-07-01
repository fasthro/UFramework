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
        private HashSet<string> assetDependenciesRecorder = new HashSet<string>();

        [ShowInInspector]
        public List<AssetBundleAssetItem> assets = new List<AssetBundleAssetItem>();

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
            assetDependenciesRecorder.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                ParseDependencies(assets[i]);
            }
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
            string dpName = "Assets/";
            if (bundleName.StartsWith("Assets/"))
            {
                return bundleName.Substring(dpName.Length);
            }
            return bundleName;
        }

        /// <summary>
        /// Add Asset Item
        /// </summary>
        /// <param name="item"></param>
        private void AddAssetItem(AssetBundleAssetItem item)
        {
            item.path = IOPath.PathUnitySeparator(item.path);
            item.size = IOPath.FileSize(item.path);

            if (!assetRecorder.Contains(item.path))
            {
                assetRecorder.Add(item.path);
                assets.Add(item);

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
        /// Buid All Bundle
        /// </summary>
        private void BuildAll()
        {
            IOPath.DirectoryClear(App.BundleDirectory);
            BuildPipeline.BuildAssetBundles(App.BundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
}
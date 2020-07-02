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

        /// <summary>
        /// 资源列表
        /// </summary>
        /// <typeparam name="AssetBundleAssetItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Assets")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnAssetsTitleBarGUI")]
        public List<AssetBundleAssetItem> assets = new List<AssetBundleAssetItem>();

        /// <summary>
        /// 依赖资源列表
        /// </summary>
        /// <typeparam name="AssetBundleAssetItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Dependencies Assets")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnAssetsDependenciesTitleBarGUI")]
        public List<AssetBundleAssetItem> dependencieAssets = new List<AssetBundleAssetItem>();

        /// <summary>
        /// 内部资源列表
        /// </summary>
        /// <typeparam name="AssetBundleAssetItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Built-In Assets")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true)]
        public List<AssetBundleAssetItem> builtInAssets = new List<AssetBundleAssetItem>();

        /// <summary>
        /// 排序类型
        /// </summary>
        [HideInInspector]
        public AssetBundleItemSortType sortType = AssetBundleItemSortType.Name;

        /// <summary>
        /// 是否为升序
        /// </summary>
        [HideInInspector]
        public bool ascendingOrder = true;

        [HideInInspector]
        public bool ascendingOrderActive = true;

        [HideInInspector]
        public bool descendingOrderActive = false;

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

        private void OnAssetsTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                var index = (int)sortType;
                var maxIndex = (int)AssetBundleItemSortType.End;
                index++;
                if (index >= maxIndex)
                {
                    sortType = AssetBundleItemSortType.Name;
                }
                else
                {
                    sortType = (AssetBundleItemSortType)index;
                }
                ascendingOrder = true;
                ascendingOrderActive = true;
                descendingOrderActive = false;
                AssetsSort(assets);
            }

            if (ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(ascendingOrderActive, EditorIcons.ArrowUp))
            {
                if (!ascendingOrder)
                {
                    AssetsSort(assets);
                }
                descendingOrderActive = false;
                ascendingOrder = true;
            }

            if (descendingOrderActive = SirenixEditorGUI.ToolbarToggle(descendingOrderActive, EditorIcons.ArrowDown))
            {
                if (ascendingOrder)
                {
                    AssetsSort(assets);
                }
                ascendingOrderActive = false;
                ascendingOrder = false;
            }
        }

        private void OnAssetsDependenciesTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                var index = (int)sortType;
                var maxIndex = (int)AssetBundleItemSortType.End;
                index++;
                if (index >= maxIndex)
                {
                    sortType = AssetBundleItemSortType.Name;
                }
                else
                {
                    sortType = (AssetBundleItemSortType)index;
                }
                ascendingOrder = true;
                ascendingOrderActive = true;
                descendingOrderActive = false;
                AssetsSort(dependencieAssets);
            }

            if (ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(ascendingOrderActive, EditorIcons.ArrowUp))
            {
                if (!ascendingOrder)
                {
                    AssetsSort(dependencieAssets);
                }
                descendingOrderActive = false;
                ascendingOrder = true;
            }

            if (descendingOrderActive = SirenixEditorGUI.ToolbarToggle(descendingOrderActive, EditorIcons.ArrowDown))
            {
                if (ascendingOrder)
                {
                    AssetsSort(dependencieAssets);
                }
                ascendingOrderActive = false;
                ascendingOrder = false;
            }
        }

        private void Refresh()
        {
            // Parse PathItemConfig
            assets.Clear();
            assetRecorder.Clear();
            var pathConfig = UConfig.Read<AssetBundleAssetPathItemConfig>();
            for (int i = 0; i < pathConfig.assetPathItems.Count; i++)
            {
                var items = ParsePathItem(pathConfig.assetPathItems[i]);
                for (int k = 0; k < items.Count; k++)
                {
                    AddAssetItem(items[k]);
                }
            }

            // Parse Dependencies
            dependencieAssets.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                ParseDependencies(assets[i]);
            }

            // Parse Built-Int
            builtInAssets.Clear();
            for (int i = 0; i < pathConfig.builtInAssetPathItems.Count; i++)
            {
                var items = ParsePathItem(pathConfig.builtInAssetPathItems[i]);
                for (int k = 0; k < items.Count; k++)
                {
                    AddBuiltInAssetItem(items[k]);
                }
            }

            // gen config relation
            var resAssetInfoConfig = UConfig.Read<ResLoaderConfig>();
            resAssetInfoConfig.assetInfoDictionary.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                resAssetInfoConfig.assetInfoDictionary.Add(assets[i].path, GenResAssetInfo(assets[i]));
            }
            for (int i = 0; i < builtInAssets.Count; i++)
            {
                resAssetInfoConfig.assetInfoDictionary.Add(builtInAssets[i].path, GenResAssetInfo(builtInAssets[i]));
            }
            resAssetInfoConfig.Save();
        }

        /// <summary>
        /// Parse Path Item
        /// </summary>
        /// <param name="pathItem"></param>
        private List<AssetBundleAssetItem> ParsePathItem(AssetBundleAssetPathItem pathItem)
        {
            List<AssetBundleAssetItem> assetItems = new List<AssetBundleAssetItem>();
            // File
            if (pathItem.buildType == AssetBundleBuildPathType.File)
            {
                var assetItem = new AssetBundleAssetItem();
                assetItem.path = pathItem.path;
                assetItem.assetBundleName = FormatAssetBundleName(pathItem.path);
                assetItem.assetType = pathItem.assetType;

                assetItems.Add(assetItem);
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

                    assetItems.Add(assetItem);
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

                    assetItems.Add(assetItem);
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

                        assetItems.Add(assetItem);
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

                        assetItems.Add(assetItem);
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

                            assetItems.Add(assetItem);
                        }
                    }
                }
            }
            return assetItems;
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
        /// Add Built-In Asset Item
        /// </summary>
        /// <param name="item"></param>
        private void AddBuiltInAssetItem(AssetBundleAssetItem item)
        {
            item.path = IOPath.PathUnitySeparator(item.path);
            item.size = IOPath.FileSize(item.path);

            builtInAssets.Add(item);
            SetAssetBundleName(item);
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
        /// 资源排序
        /// </summary>
        /// <param name="items"></param>
        private void AssetsSort(List<AssetBundleAssetItem> items)
        {
            if (sortType == AssetBundleItemSortType.Name)
            {
                if (ascendingOrder)
                {
                    items.Sort((x, y) => x.path.CompareTo(y.path));
                }
                else
                {
                    items.Sort((x, y) => y.path.CompareTo(x.path));
                }
            }
            else if (sortType == AssetBundleItemSortType.Type)
            {
                if (ascendingOrder)
                {
                    items.Sort((x, y) => x.assetType.CompareTo(y.assetType));
                }
                else
                {
                    items.Sort((x, y) => y.assetType.CompareTo(x.assetType));
                }
            }
            else if (sortType == AssetBundleItemSortType.Size)
            {
                if (ascendingOrder)
                {
                    items.Sort((x, y) => x.size.CompareTo(y.size));
                }
                else
                {
                    items.Sort((x, y) => y.size.CompareTo(x.size));
                }
            }
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
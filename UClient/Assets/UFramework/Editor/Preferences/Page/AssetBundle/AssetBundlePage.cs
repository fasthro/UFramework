/*
 * @Author: fasthro
 * @Date: 2020-06-29 17:00:30
 * @Description: AssetBundle
 */
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UFramework.ResLoader;
using UFramework.Tools;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.Assets
{
    public class AssetBundlePage : IPage, IPageBar
    {
        public string menuName { get { return "AssetBundle"; } }
        static AssetBundle_AssetConfig describeObject;
        static AppConfig app;

        [BoxGroup("General Setting")]
        public int version;

        /// <summary>
        /// AssetBundle列表
        /// </summary>
        /// <typeparam name="BundleItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("AssetBundle")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnBundlesTitleBarGUI")]
        [LabelText("AssetBundles")]
        public List<BundleItem> bundles = new List<BundleItem>();

        /// <summary>
        /// 资源列表
        /// </summary>
        /// <typeparam name="BundleAssetItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Assets")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnAssetsTitleBarGUI")]
        [LabelText("All Assets")]
        public List<BundleAssetItem> assets = new List<BundleAssetItem>();

        /// <summary>
        /// 依赖资源列表
        /// </summary>
        /// <typeparam name="BundleAssetItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Assets")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnDependenciesTitleBarGUI")]
        [LabelText("Dependencies Assets")]
        public List<BundleAssetItem> dependencieAssets = new List<BundleAssetItem>();

        /// <summary>
        /// 内部资源列表
        /// </summary>
        /// <typeparam name="BundleAssetItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Assets")]
        [ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnBuiltInTitleBarGUI")]
        [LabelText("Built-In Assets")]
        public List<BundleAssetItem> builtInAssets = new List<BundleAssetItem>();

        // shader bundle
        private BundleItem shaderBundle = new BundleItem();

        // temp
        private HashSet<string> assetTracker = new HashSet<string>();
        private Dictionary<string, BundleItem> bundleTracker = new Dictionary<string, BundleItem>();
        private HashSet<string> dependenciesTracker = new HashSet<string>();
        private Dictionary<string, HashSet<string>> dependenciesAnalysisTracker = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<string>> dependenciesBundleAnalysisTracker = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// 排序类型
        /// </summary>
        [HideInInspector]
        public SortType sortType = SortType.Title;

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
            app = UConfig.Read<AppConfig>();
            describeObject = UConfig.Read<AssetBundle_AssetConfig>();
            assets = describeObject.assets;
            AnalysisAssets();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Assest Bundle")))
            {
                BuildAssetsBundle(true);
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Force Build Assest Bundle")))
            {
                BuildAssetsBundle(false);
            }

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                AnalysisAssets();
            }
        }

        public void OnSaveDescribe()
        {
            describeObject.assets = assets;
            describeObject.Save();
        }

        private void OnBundlesTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Title, "Title Sort"))
            {
                sortType = SortType.Title;
                SortBundleList(bundles);
            }

            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Size, "File Size Sort"))
            {
                sortType = SortType.Size;
                SortBundleList(bundles);
            }

            if (ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(ascendingOrderActive, EditorIcons.ArrowUp))
            {
                if (!ascendingOrder)
                {
                    SortBundleList(bundles);
                }
                descendingOrderActive = false;
                ascendingOrder = true;
            }

            if (descendingOrderActive = SirenixEditorGUI.ToolbarToggle(descendingOrderActive, EditorIcons.ArrowDown))
            {
                if (ascendingOrder)
                {
                    SortBundleList(bundles);
                }
                ascendingOrderActive = false;
                ascendingOrder = false;
            }
        }

        private void OnAssetsTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Title, "Title Sort"))
            {
                sortType = SortType.Title;
                SortBundleAssetList(assets);
            }

            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Size, "File Size Sort"))
            {
                sortType = SortType.Size;
                SortBundleAssetList(assets);
            }

            if (ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(ascendingOrderActive, EditorIcons.ArrowUp))
            {
                if (!ascendingOrder)
                {
                    SortBundleAssetList(assets);
                }
                descendingOrderActive = false;
                ascendingOrder = true;
            }

            if (descendingOrderActive = SirenixEditorGUI.ToolbarToggle(descendingOrderActive, EditorIcons.ArrowDown))
            {
                if (ascendingOrder)
                {
                    SortBundleAssetList(assets);
                }
                ascendingOrderActive = false;
                ascendingOrder = false;
            }
        }

        private void OnDependenciesTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Title, "Title Sort"))
            {
                sortType = SortType.Title;
                SortBundleAssetList(dependencieAssets);
            }

            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Size, "File Size Sort"))
            {
                sortType = SortType.Size;
                SortBundleAssetList(dependencieAssets);
            }

            if (ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(ascendingOrderActive, EditorIcons.ArrowUp))
            {
                if (!ascendingOrder)
                {
                    SortBundleAssetList(dependencieAssets);
                }
                descendingOrderActive = false;
                ascendingOrder = true;
            }

            if (descendingOrderActive = SirenixEditorGUI.ToolbarToggle(descendingOrderActive, EditorIcons.ArrowDown))
            {
                if (ascendingOrder)
                {
                    SortBundleAssetList(dependencieAssets);
                }
                ascendingOrderActive = false;
                ascendingOrder = false;
            }
        }

        private void OnBuiltInTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Title, "Title Sort"))
            {
                sortType = SortType.Title;
                SortBundleAssetList(builtInAssets);
            }

            if (SirenixEditorGUI.ToolbarToggle(sortType == SortType.Size, "File Size Sort"))
            {
                sortType = SortType.Size;
                SortBundleAssetList(builtInAssets);
            }

            if (ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(ascendingOrderActive, EditorIcons.ArrowUp))
            {
                if (!ascendingOrder)
                {
                    SortBundleAssetList(builtInAssets);
                }
                descendingOrderActive = false;
                ascendingOrder = true;
            }

            if (descendingOrderActive = SirenixEditorGUI.ToolbarToggle(descendingOrderActive, EditorIcons.ArrowDown))
            {
                if (ascendingOrder)
                {
                    SortBundleAssetList(builtInAssets);
                }
                ascendingOrderActive = false;
                ascendingOrder = false;
            }
        }

        /// <summary>
        /// 分析资源
        /// </summary>
        private void AnalysisAssets()
        {
            // analysis search path
            assets.Clear();
            assetTracker.Clear();
            var pathConfig = UConfig.Read<AssetBundle_AssetSearchPathConfig>();
            for (int i = 0; i < pathConfig.assetPathItems.Count; i++)
            {
                var items = ParsePathItem(pathConfig.assetPathItems[i]);
                for (int k = 0; k < items.Count; k++)
                {
                    var item = items[k];
                    item.path = IOPath.PathUnitySeparator(item.path);
                    item.size = IOPath.FileSize(item.path);

                    if (!assetTracker.Contains(item.path))
                    {
                        assetTracker.Add(item.path);
                        assets.Add(item);
                        SetAssetBundleName(item);
                    }
                }
            }

            // analysis built-in
            builtInAssets.Clear();
            for (int i = 0; i < pathConfig.builtInAssetPathItems.Count; i++)
            {
                var items = ParsePathItem(pathConfig.builtInAssetPathItems[i]);
                for (int k = 0; k < items.Count; k++)
                {
                    var item = items[k];
                    item.path = IOPath.PathUnitySeparator(item.path);
                    item.size = IOPath.FileSize(item.path);
                    builtInAssets.Add(item);

                    SetAssetBundleName(item);
                }
            }

            // analysis bundles
            bundleTracker.Clear();
            bundles.Clear();
            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];
                BundleItem bundle;
                if (!bundleTracker.TryGetValue(asset.bundleName, out bundle))
                {
                    bundle = new BundleItem();
                    bundle.displayBundleName = asset.displayBundleName;
                    bundle.assets = new List<BundleAssetItem>();
                    bundleTracker.Add(asset.bundleName, bundle);

                    bundles.Add(bundle);
                }
                bundle.size += asset.size;
                bundle.assets.Add(asset);
            }

            // analysis dependencies
            dependencieAssets.Clear();
            dependenciesTracker.Clear();
            dependenciesAnalysisTracker.Clear();
            for (int i = 0; i < bundles.Count; i++)
            {
                var bundle = bundles[i];
                var dependencies = AssetDatabase.GetDependencies(bundle.GetAssetPaths().ToArray(), true);
                if (dependencies.Length > 0)
                {
                    foreach (var asset in dependencies)
                    {
                        if (ValidateAssetPath(asset))
                        {
                            HashSet<string> temps;
                            if (!dependenciesAnalysisTracker.TryGetValue(asset, out temps))
                            {
                                temps = new HashSet<string>();
                                dependenciesAnalysisTracker.Add(asset, temps);
                            }
                            temps.Add(bundle.displayBundleName);
                            if (temps.Count > 1)
                            {
                                if (!dependenciesTracker.Contains(asset))
                                {
                                    var pathItem = new AssetSearchItem();
                                    pathItem.path = asset;
                                    pathItem.nameType = NameType.Path;

                                    var assetItem = new BundleAssetItem();
                                    assetItem.path = IOPath.PathUnitySeparator(pathItem.path);
                                    assetItem.displayBundleName = FormatAssetBundleName(pathItem, pathItem.path);
                                    assetItem.size = IOPath.FileSize(pathItem.path);

                                    dependenciesTracker.Add(asset);
                                    dependencieAssets.Add(assetItem);
                                }
                            }
                        }
                    }
                }
            }

            // analysis dependencies bundles
            dependenciesBundleAnalysisTracker.Clear();
            for (int i = 0; i < dependencieAssets.Count; i++)
            {
                var asset = dependencieAssets[i];
                var dependencies = AssetDatabase.GetDependencies(asset.path, true);

                HashSet<string> temps;
                if (!dependenciesBundleAnalysisTracker.TryGetValue(asset.path, out temps))
                {
                    temps = new HashSet<string>();
                    dependenciesBundleAnalysisTracker.Add(asset.path, temps);
                }

                foreach (var file in dependencies)
                {
                    if (!asset.path.Equals(file))
                    {
                        temps.Add(file);
                    }
                }
            }

            List<BundleAssetItem> removeDps = new List<BundleAssetItem>();
            for (int i = 0; i < dependencieAssets.Count; i++)
            {
                var tAsset = dependencieAssets[i];
                bool hasRef = false;
                foreach (KeyValuePair<string, HashSet<string>> asset in dependenciesBundleAnalysisTracker)
                {
                    foreach (var item in asset.Value)
                    {
                        if (tAsset.path.Equals(item))
                        {
                            hasRef = true;
                            break;
                        }
                    }
                    if (hasRef) break;
                }
                if (hasRef)
                {
                    removeDps.Add(tAsset);
                }
            }

            foreach (var item in removeDps)
            {
                dependencieAssets.Remove(item);
            }

            foreach (var asset in dependencieAssets)
            {
                // assets
                if (!assetTracker.Contains(asset.path))
                {
                    var pathItem = new AssetSearchItem();
                    pathItem.path = asset.path;
                    pathItem.nameType = NameType.Path;

                    var assetItem = new BundleAssetItem();
                    assetItem.path = IOPath.PathUnitySeparator(pathItem.path);
                    assetItem.displayBundleName = FormatAssetBundleName(pathItem, pathItem.path);
                    assetItem.size = IOPath.FileSize(pathItem.path);
                    assetItem.IsDependencies = true;

                    assetTracker.Add(asset.path);

                    assets.Add(assetItem);
                }

                // bundles
                BundleItem bundle;
                if (!bundleTracker.TryGetValue(asset.bundleName, out bundle))
                {
                    bundle = new BundleItem();
                    bundle.displayBundleName = asset.displayBundleName;
                    bundle.assets = new List<BundleAssetItem>();
                    bundle.IsDependencies = true;
                    bundleTracker.Add(asset.bundleName, bundle);

                    bundles.Add(bundle);
                }
                bundle.size += asset.size;
                bundle.assets.Add(asset);

                // bundle name
                SetAssetBundleName(asset);
                var dependencies = AssetDatabase.GetDependencies(asset.path, true);
                foreach (var dAsset in dependencies)
                {
                    var dItem = new BundleAssetItem();
                    dItem.path = dAsset;
                    dItem.displayBundleName = asset.displayBundleName;
                    SetAssetBundleName(dItem);
                }
            }

            // optimiz shader package
            // shader
            shaderBundle = new BundleItem();
            shaderBundle.displayBundleName = "shaders";
            shaderBundle.assets = new List<BundleAssetItem>();

            foreach (var bundle in bundles)
            {
                var dependencies = AssetDatabase.GetDependencies(bundle.GetAssetPaths().ToArray(), true);
                foreach (var asset in dependencies)
                {
                    if (asset.EndsWith(".shader"))
                    {
                        if (!assetTracker.Contains(asset))
                        {
                            var shaderAsset = new BundleAssetItem();
                            shaderAsset.path = asset;
                            shaderAsset.size = IOPath.FileSize(asset);
                            shaderAsset.displayBundleName = "shaders";

                            assetTracker.Add(asset);
                            assets.Add(shaderAsset);

                            shaderBundle.assets.Add(shaderAsset);

                            SetAssetBundleName(shaderAsset);
                        }
                    }
                }
            }

            bundles.Add(shaderBundle);
        }

        /// <summary>
        /// Parse Path Item
        /// </summary>
        /// <param name="pathItem"></param>
        private List<BundleAssetItem> ParsePathItem(AssetSearchItem pathItem)
        {
            List<BundleAssetItem> assetItems = new List<BundleAssetItem>();
            var fileExists = IOPath.FileExists(pathItem.path);
            var directoryExists = IOPath.DirectoryExists(pathItem.path);
            if (fileExists && ValidateAssetPath(pathItem.path))
            {
                var assetItem = new BundleAssetItem();
                assetItem.path = IOPath.PathUnitySeparator(pathItem.path);
                assetItem.displayBundleName = FormatAssetBundleName(pathItem, pathItem.path);
                assetItems.Add(assetItem);
            }
            else if (directoryExists)
            {
                var patterns = pathItem.pattern.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in patterns)
                {
                    var files = Directory.GetFiles(pathItem.path, item, SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (Directory.Exists(file)) continue;
                        var ext = Path.GetExtension(file).ToLower();
                        if ((ext == ".fbx" || ext == ".anim") && !item.Contains(ext)) continue;
                        if (!ValidateAssetPath(file)) continue;

                        var assetItem = new BundleAssetItem();
                        assetItem.path = IOPath.PathUnitySeparator(file);
                        assetItem.displayBundleName = FormatAssetBundleName(pathItem, file);
                        assetItems.Add(assetItem);
                    }
                }
            }
            return assetItems;
        }

        /// <summary>
        /// 验证资源路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool ValidateAssetPath(string path)
        {
            if (!path.StartsWith("Assets/")) return false;

            var ext = Path.GetExtension(path).ToLower();
            return ext != ".dll" && ext != ".cs" && ext != ".meta" && ext != ".js" && ext != ".boo";
        }

        /// <summary>
        /// 格式化 AssetBundleName
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private string FormatAssetBundleName(AssetSearchItem item, string filePath)
        {
            filePath = IOPath.PathUnitySeparator(filePath);
            var searchPath = IOPath.PathUnitySeparator(item.path);
            if (item.nameType == NameType.Path)
            {
                return filePath;
            }
            else if (item.nameType == NameType.Directory)
            {
                var sps = searchPath.Split(Path.AltDirectorySeparatorChar);
                var fps = filePath.Split(Path.AltDirectorySeparatorChar);
                if (sps.Length + 1 <= fps.Length)
                {
                    return IOPath.PathCombine(searchPath, fps[sps.Length]);
                }
            }
            else if (item.nameType == NameType.TopDirectory)
            {
                return searchPath;
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置 AssetBundle Name
        /// </summary>
        /// <param name="assetItem"></param>
        private void SetAssetBundleName(BundleAssetItem assetItem)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetItem.path);
            assetImporter.assetBundleName = assetItem.displayBundleName;
        }

        /// <summary>
        /// 生成 ResAssetInfo
        /// </summary>
        /// <param name="assetItem"></param>
        /// <returns></returns>
        private ResAssetInfo GenResAssetInfo(BundleAssetItem assetItem)
        {
            var resInfo = new ResAssetInfo();
            resInfo.assetBundleName = assetItem.displayBundleName;
            resInfo.size = assetItem.size;
            resInfo.md5 = IOPath.FileMD5(assetItem.path);
            return resInfo;
        }

        /// <summary>
        /// Bundle列表排序
        /// </summary>
        /// <param name="items"></param>
        private void SortBundleList(List<BundleItem> items)
        {
            if (sortType == SortType.Title)
            {
                if (ascendingOrder)
                    items.Sort((a, b) => string.Compare(a.displayBundleName, b.displayBundleName, StringComparison.Ordinal));
                else
                    items.Sort((a, b) => string.Compare(b.displayBundleName, a.displayBundleName, StringComparison.Ordinal));
            }
            else if (sortType == SortType.Size)
            {
                if (ascendingOrder)
                    items.Sort((x, y) => x.size.CompareTo(y.size));
                else
                    items.Sort((x, y) => y.size.CompareTo(x.size));
            }
        }

        /// <summary>
        /// BundleAsset列表排序
        /// </summary>
        /// <param name="items"></param>
        private void SortBundleAssetList(List<BundleAssetItem> items)
        {
            if (sortType == SortType.Title)
            {
                if (ascendingOrder)
                    items.Sort((a, b) => string.Compare(a.path, b.path, StringComparison.Ordinal));
                else
                    items.Sort((a, b) => string.Compare(b.path, a.path, StringComparison.Ordinal));
            }
            else if (sortType == SortType.Size)
            {
                if (ascendingOrder)
                    items.Sort((x, y) => x.size.CompareTo(y.size));
                else
                    items.Sort((x, y) => y.size.CompareTo(x.size));
            }
        }


        /// <summary>
        /// Buid Assets Bundle
        /// </summary>
        /// <param name="clean"></param>
        private void BuildAssetsBundle(bool clean)
        {
            IOPath.DirectoryClear(App.BundleDirectory);
            if (clean)
            {
                IOPath.DirectoryClear(App.BundleTempDirectory);
            }
            else
            {
                if (!IOPath.DirectoryExists(App.BundleTempDirectory))
                {
                    IOPath.DirectoryClear(App.BundleTempDirectory);
                }
            }

            AssetBundleBuild[] builds = GetAssetBundleBuilds();
            var assetBundleManifest = BuildPipeline.BuildAssetBundles(App.BundleTempDirectory, builds, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            if (assetBundleManifest == null)
            {
                return;
            }

            AssetDatabase.Refresh();

            // var zipfile = IOPath.PathCombine(App.DataDirectory, "res.zip");
            // IOPath.FileDelete(zipfile);
            // IOPath.DirectoryDelete(App.BundleDirectory);
            // if (app.isDevelopmentVersion)
            // {
            //     IOPath.DirectoryCopy(App.BundleTempDirectory, App.BundleDirectory);
            // }
            // else
            // {
            //     UZip.Zip(new string[] { App.BundleTempDirectory }, zipfile);
            // }
            // AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// 获取打包数据
        /// </summary>
        /// <returns></returns>
        private AssetBundleBuild[] GetAssetBundleBuilds()
        {
            var builds = new List<AssetBundleBuild>();
            foreach (var bundle in bundles)
            {
                builds.Add(new AssetBundleBuild
                {
                    assetNames = bundle.GetAssetPaths().ToArray(),
                    assetBundleName = bundle.bundleName
                });
            }
            return builds.ToArray();
        }
    }
}
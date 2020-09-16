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
        [BoxGroup("General Setting")]
        public bool buildNameHash = true;
        [BoxGroup("General Setting")]
        public bool importerBundleName = false;

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
        private Dictionary<string, BundleAssetItem> assetTracker = new Dictionary<string, BundleAssetItem>();
        private Dictionary<string, BundleItem> bundleTracker = new Dictionary<string, BundleItem>();
        private HashSet<string> dependenciesTracker = new HashSet<string>();
        private Dictionary<string, HashSet<string>> dependenciesAnalysisTracker = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, HashSet<string>> dependenciesBundleAnalysisTracker = new Dictionary<string, HashSet<string>>();
        private HashSet<string> dependenciesRefTracker = new HashSet<string>();

        // sort ruler
        private SortRuler bundleSortRuler;
        private SortRuler assetSortRuler;
        private SortRuler dependenciesAssetSortRuler;
        private SortRuler builtInAssetSortRuler;

        public object GetInstance()
        {
            return this;
        }
        public void OnRenderBefore()
        {
            bundleSortRuler = new SortRuler(bundles);
            assetSortRuler = new SortRuler(assets);
            dependenciesAssetSortRuler = new SortRuler(dependencieAssets);
            builtInAssetSortRuler = new SortRuler(builtInAssets);

            app = UConfig.Read<AppConfig>();
            describeObject = UConfig.Read<AssetBundle_AssetConfig>();
            bundles = describeObject.bundles;
            assets = describeObject.assets;
            dependencieAssets = describeObject.dependencieAssets;
            builtInAssets = describeObject.builtInAssets;

            if (bundles.Count <= 0)
            {
                AnalysisAssets();
            }
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Assest Bundle")))
            {
                BuildAssetsBundle(false);
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Force Build Assest Bundle")))
            {
                BuildAssetsBundle(true);
            }

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                AnalysisAssets();
            }
        }

        public void OnSaveDescribe()
        {
            describeObject.bundles = bundles;
            describeObject.assets = assets;
            describeObject.dependencieAssets = dependencieAssets;
            describeObject.builtInAssets = builtInAssets;
            describeObject.Save();
        }

        private void OnBundlesTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(bundleSortRuler.sortType == SortType.Title, "Title"))
                bundleSortRuler.sortType = SortType.Title;

            if (SirenixEditorGUI.ToolbarToggle(bundleSortRuler.sortType == SortType.FileSize, "File Size"))
                bundleSortRuler.sortType = SortType.FileSize;

            if (SirenixEditorGUI.ToolbarToggle(bundleSortRuler.sortType == SortType.BundleSize, "Bundle Size"))
                bundleSortRuler.sortType = SortType.BundleSize;

            bundleSortRuler.ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(bundleSortRuler.ascendingOrderActive, EditorIcons.ArrowUp);
            bundleSortRuler.descendingOrderActive = SirenixEditorGUI.ToolbarToggle(bundleSortRuler.descendingOrderActive, EditorIcons.ArrowDown);
        }

        private void OnAssetsTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(assetSortRuler.sortType == SortType.Title, "Title"))
                assetSortRuler.sortType = SortType.Title;

            if (SirenixEditorGUI.ToolbarToggle(assetSortRuler.sortType == SortType.FileSize, "File Size"))
                assetSortRuler.sortType = SortType.FileSize;

            assetSortRuler.ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(assetSortRuler.ascendingOrderActive, EditorIcons.ArrowUp);
            assetSortRuler.descendingOrderActive = SirenixEditorGUI.ToolbarToggle(assetSortRuler.descendingOrderActive, EditorIcons.ArrowDown);
        }

        private void OnDependenciesTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(dependenciesAssetSortRuler.sortType == SortType.Title, "Title"))
                dependenciesAssetSortRuler.sortType = SortType.Title;

            if (SirenixEditorGUI.ToolbarToggle(dependenciesAssetSortRuler.sortType == SortType.FileSize, "File Size"))
                dependenciesAssetSortRuler.sortType = SortType.FileSize;

            dependenciesAssetSortRuler.ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(dependenciesAssetSortRuler.ascendingOrderActive, EditorIcons.ArrowUp);
            dependenciesAssetSortRuler.descendingOrderActive = SirenixEditorGUI.ToolbarToggle(dependenciesAssetSortRuler.descendingOrderActive, EditorIcons.ArrowDown);
        }

        private void OnBuiltInTitleBarGUI()
        {
            if (SirenixEditorGUI.ToolbarToggle(builtInAssetSortRuler.sortType == SortType.Title, "Title"))
                builtInAssetSortRuler.sortType = SortType.Title;

            if (SirenixEditorGUI.ToolbarToggle(builtInAssetSortRuler.sortType == SortType.FileSize, "File Size"))
                builtInAssetSortRuler.sortType = SortType.FileSize;

            builtInAssetSortRuler.ascendingOrderActive = SirenixEditorGUI.ToolbarToggle(builtInAssetSortRuler.ascendingOrderActive, EditorIcons.ArrowUp);
            builtInAssetSortRuler.descendingOrderActive = SirenixEditorGUI.ToolbarToggle(builtInAssetSortRuler.descendingOrderActive, EditorIcons.ArrowDown);
        }

        /// <summary>
        /// 分析资源
        /// </summary>
        private void AnalysisAssets()
        {
            // remove bundle name
            if (importerBundleName)
            {
                AssetBundlePreferencesPage.RemoveAssetBundleName();
            }

            // progress
            int progress = 0;
            string progressTitle = "";
            string progressDes = "";

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

                    if (!assetTracker.ContainsKey(item.path))
                    {
                        assetTracker.Add(item.path, item);
                        assets.Add(item);
                    }

                    progressTitle = "analysis search path";
                    progress = k;
                    progressDes = item.path;
                    Utils.UpdateProgress(progressTitle, progressDes, progress, items.Count);
                }
            }

            for (int i = 0; i < pathConfig.assetFileItems.Count; i++)
            {
                var fileItem = pathConfig.assetFileItems[i];
                if (ValidateAssetPath(fileItem.path))
                {
                    var pathItem = new AssetSearchItem();
                    pathItem.path = fileItem.path;
                    pathItem.nameType = fileItem.nameType;

                    var item = new BundleAssetItem();
                    item.path = IOPath.PathUnitySeparator(pathItem.path);
                    item.bundleName = FormatAssetBundleName(pathItem, pathItem.path);
                    item.size = IOPath.FileSize(item.path);

                    if (!assetTracker.ContainsKey(item.path))
                    {
                        assetTracker.Add(item.path, item);
                        assets.Add(item);
                    }
                }

                progressTitle = "analysis file path";
                progress = i;
                progressDes = fileItem.path;
                Utils.UpdateProgress(progressTitle, progressDes, progress, pathConfig.assetFileItems.Count);
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

                    progressTitle = "analysis built-in";
                    progress = k;
                    progressDes = item.path;
                    Utils.UpdateProgress(progressTitle, progressDes, progress, items.Count);
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
                    bundle.bundleName = asset.bundleName;
                    bundle.assets = new List<BundleAssetItem>();
                    bundle.bundleSize = GetBuildBundleSize(bundle);
                    bundleTracker.Add(asset.bundleName, bundle);

                    bundles.Add(bundle);
                }
                bundle.fileSize += asset.size;
                bundle.assets.Add(asset);

                progressTitle = "analysis bundle";
                progress = i;
                progressDes = asset.path;
                Utils.UpdateProgress(progressTitle, progressDes, progress, assets.Count);
            }

            // analysis dependencies
            dependencieAssets.Clear();
            dependenciesTracker.Clear();
            dependenciesAnalysisTracker.Clear();
            for (int i = 0; i < bundles.Count; i++)
            {
                progress = 0;

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
                            temps.Add(bundle.bundleName);
                            if (temps.Count > 1)
                            {
                                if (!dependenciesTracker.Contains(asset))
                                {

                                    var pathItem = new AssetSearchItem();
                                    pathItem.path = asset;
                                    pathItem.nameType = NameType.Path;

                                    var assetItem = new BundleAssetItem();
                                    assetItem.path = IOPath.PathUnitySeparator(pathItem.path);
                                    assetItem.bundleName = FormatAssetBundleName(pathItem, pathItem.path);
                                    assetItem.size = IOPath.FileSize(pathItem.path);

                                    dependenciesTracker.Add(asset);
                                    dependencieAssets.Add(assetItem);
                                }
                            }

                            // asset ref count
                            dependenciesRefTracker.Add(asset);

                            progressTitle = "analysis bundle dependencies";
                            progress++;
                            progressDes = asset;
                            Utils.UpdateProgress(progressTitle, progressDes, progress, dependencies.Length);
                        }
                    }
                }
            }

            // analysis dependencies bundles
            dependenciesBundleAnalysisTracker.Clear();
            for (int i = 0; i < dependencieAssets.Count; i++)
            {
                progress = 0;

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

                    progressTitle = "analysis dependencies";
                    progress++;
                    progressDes = file;
                    Utils.UpdateProgress(progressTitle, progressDes, progress, dependencies.Length);
                }
            }

            // 合并依赖资源
            // 如果有材质球引用贴图资源，那么贴图资源不会单独打包
            List<BundleAssetItem> removeDps = new List<BundleAssetItem>();
            for (int i = 0; i < dependencieAssets.Count; i++)
            {
                var tAsset = dependencieAssets[i];
                if (tAsset.isTexture)
                {
                    int refCount = 0;
                    foreach (KeyValuePair<string, HashSet<string>> asset in dependenciesBundleAnalysisTracker)
                    {
                        foreach (var item in asset.Value)
                        {
                            if (tAsset.path.Equals(item))
                            {
                                refCount++;
                            }
                        }
                    }
                    if (refCount == 1 && !dependenciesRefTracker.Contains(tAsset.path))
                    {
                        removeDps.Add(tAsset);
                    }
                }
            }

            // 如果有Prefab引用材质球，那么材质球不会单独打包
            for (int i = 0; i < dependencieAssets.Count; i++)
            {
                var tAsset = dependencieAssets[i];
                if (tAsset.isMaterial)
                {
                    int refCount = 0;
                    foreach (KeyValuePair<string, HashSet<string>> asset in dependenciesBundleAnalysisTracker)
                    {
                        foreach (var item in asset.Value)
                        {
                            if (tAsset.path.Equals(item))
                            {
                                refCount++;
                            }
                        }
                    }
                    if (refCount == 1 && !dependenciesRefTracker.Contains(tAsset.path))
                    {
                        removeDps.Add(tAsset);
                    }
                }
            }

            foreach (var item in removeDps)
            {
                dependencieAssets.Remove(item);
            }

            progress = 0;
            foreach (var asset in dependencieAssets)
            {
                BundleAssetItem assetItem;
                BundleItem bundleItem;
                // 剔除已存在bundle中的资源，使其单独打包
                if (assetTracker.TryGetValue(asset.path, out assetItem))
                {
                    assets.Remove(assetItem);
                    assetTracker.Remove(asset.path);
                    if (bundleTracker.TryGetValue(assetItem.bundleName, out bundleItem))
                    {
                        bundleItem.assets.Remove(assetItem);
                        if (bundleItem.assets.Count <= 0)
                        {
                            bundles.Remove(bundleItem);
                            bundleTracker.Remove(assetItem.bundleName);
                        }
                    }
                }

                // assets
                var pathItem = new AssetSearchItem();
                pathItem.path = asset.path;
                pathItem.nameType = NameType.Path;

                assetItem = new BundleAssetItem();
                assetItem.path = IOPath.PathUnitySeparator(pathItem.path);
                assetItem.bundleName = FormatAssetBundleName(pathItem, pathItem.path);
                assetItem.size = IOPath.FileSize(pathItem.path);
                assetItem.IsDependencies = true;

                assetTracker.Add(asset.path, asset);
                assets.Add(assetItem);

                // bundles
                if (!bundleTracker.TryGetValue(asset.bundleName, out bundleItem))
                {
                    bundleItem = new BundleItem();
                    bundleItem.bundleName = asset.bundleName;
                    bundleItem.assets = new List<BundleAssetItem>();
                    bundleItem.IsDependencies = true;
                    bundleTracker.Add(asset.bundleName, bundleItem);

                    bundles.Add(bundleItem);
                }
                bundleItem.fileSize += asset.size;
                bundleItem.bundleSize = GetBuildBundleSize(bundleItem);
                bundleItem.assets.Add(asset);

                progressTitle = "analysis dependencies";
                progress++;
                progressDes = asset.path;
                Utils.UpdateProgress(progressTitle, progressDes, progress, dependencieAssets.Count);
            }

            // built-In
            // foreach (var asset in builtInAssets)
            // {
            //     var item = new BundleAssetItem();

            // }

            // optimiz shader package
            // shader
            shaderBundle = new BundleItem();
            shaderBundle.bundleName = "shaders";
            shaderBundle.assets = new List<BundleAssetItem>();

            foreach (var bundle in bundles)
            {
                progress = 0;

                var dependencies = AssetDatabase.GetDependencies(bundle.GetAssetPaths().ToArray(), true);
                foreach (var asset in dependencies)
                {
                    if (asset.EndsWith(".shader"))
                    {
                        if (!assetTracker.ContainsKey(asset))
                        {
                            var shaderAsset = new BundleAssetItem();
                            shaderAsset.path = asset;
                            shaderAsset.size = IOPath.FileSize(asset);
                            shaderAsset.bundleName = "shaders";

                            assetTracker.Add(asset, shaderAsset);
                            assets.Add(shaderAsset);

                            shaderBundle.fileSize += shaderAsset.size;
                            shaderBundle.assets.Add(shaderAsset);
                        }
                    }

                    progressTitle = "optimiz bundle";
                    progress++;
                    progressDes = asset;
                    Utils.UpdateProgress(progressTitle, progressDes, progress, dependencieAssets.Count);
                }
            }
            shaderBundle.bundleSize = GetBuildBundleSize(shaderBundle);
            if (shaderBundle.assets.Count > 0)
            {
                bundles.Add(shaderBundle);
            }

            // remove empty bundle
            List<BundleItem> removes = new List<BundleItem>();
            foreach (var bundle in bundles)
            {
                if (bundle.assets.Count <= 0)
                {
                    removes.Add(bundle);
                }
            }
            foreach (var bundle in removes)
            {
                removes.Remove(bundle);
                bundleTracker.Remove(bundle.bundleName);
            }

            // bundle name
            if (importerBundleName)
            {
                for (int i = 0; i < assets.Count; i++)
                {
                    var assetItem = assets[i];
                    UnityEditor.AssetImporter assetImporter = UnityEditor.AssetImporter.GetAtPath(assetItem.path);
                    assetImporter.assetBundleName = GetBuildBundleName(assetItem.bundleName);

                    progressTitle = "bundle name";
                    progress = i;
                    progressDes = assetItem.path;
                    Utils.UpdateProgress(progressTitle, progressDes, progress, dependencieAssets.Count);
                }
            }
            
            Utils.HideProgress();
        }

        /// <summary>
        /// Parse Path Item
        /// </summary>
        /// <param name="pathItem"></param>
        private List<BundleAssetItem> ParsePathItem(AssetSearchItem pathItem)
        {
            List<BundleAssetItem> assetItems = new List<BundleAssetItem>();
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
                    assetItem.bundleName = FormatAssetBundleName(pathItem, file);
                    assetItems.Add(assetItem);
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
        /// 获取打包的 bundle 名称
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        private string GetBuildBundleName(string bundle)
        {
            if (buildNameHash) return UHash.GetMD5Hash(bundle) + App.AssetBundleExtension;
            return bundle.ToLower() + App.AssetBundleExtension;
        }

        /// <summary>
        /// 获取 bundle size
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        private long GetBuildBundleSize(BundleItem bundle)
        {
            var path = IOPath.PathCombine(App.BundleTempDirectory, GetBuildBundleName(bundle.bundleName));
            if (IOPath.FileExists(path))
                return IOPath.FileSize(path);
            return 0;
        }

        /// <summary>
        /// 生成 ResAssetInfo
        /// </summary>
        /// <param name="assetItem"></param>
        /// <returns></returns>
        private ResAssetInfo GenResAssetInfo(BundleAssetItem assetItem)
        {
            var resInfo = new ResAssetInfo();
            resInfo.assetBundleName = assetItem.bundleName;
            resInfo.size = assetItem.size;
            resInfo.md5 = IOPath.FileMD5(assetItem.path);
            return resInfo;
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

            BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
            AssetBundleBuild[] builds = GetAssetBundleBuilds();
            var assetBundleManifest = BuildPipeline.BuildAssetBundles(App.BundleTempDirectory, builds, options, EditorUserBuildSettings.activeBuildTarget);
            if (assetBundleManifest == null) return;

            // manifest
            var manifest = GetAsset<Manifest>(Manifest.AssetPath);

            var bundle2Ids = new Dictionary<string, int>();
            var assetBundles = assetBundleManifest.GetAllAssetBundles();
            for (int i = 0; i < assetBundles.Length; i++)
            {
                bundle2Ids[assetBundles[i]] = i;
            }

            var bundleRefs = new List<BundleRef>();
            for (int i = 0; i < assetBundles.Length; i++)
            {
                var bundle = assetBundles[i];
                var dependencies = assetBundleManifest.GetAllDependencies(bundle);
                var path = IOPath.PathCombine(App.BundleTempDirectory, bundle);
                if (File.Exists(path))
                {
                    using (var stream = File.OpenRead(path))
                    {
                        bundleRefs.Add(new BundleRef
                        {
                            name = bundle,
                            id = i,
                            dependencies = Array.ConvertAll(dependencies, input => bundle2Ids[input]),
                            size = stream.Length,
                            hash = assetBundleManifest.GetAssetBundleHash(bundle).ToString(),
                        });
                    }
                }
                else
                {
                    Debug.LogError(path + " file not exsit.");
                }
            }

            var dirs = new List<string>();
            var assetRefs = new List<AssetRef>();
            for (var i = 0; i < assets.Count; i++)
            {
                var item = assets[i];
                var path = item.path;
                var dir = IOPath.PathUnitySeparator(Path.GetDirectoryName(path));
                var index = dirs.FindIndex(o => o.Equals(dir));
                if (index == -1)
                {
                    index = dirs.Count;
                    dirs.Add(dir);
                }
                var asset = new AssetRef { bundle = bundle2Ids[GetBuildBundleName(item.bundleName)], dir = index, name = Path.GetFileName(path) };
                assetRefs.Add(asset);
            }

            manifest.dirs = dirs.ToArray();
            manifest.assets = assetRefs.ToArray();
            manifest.bundles = bundleRefs.ToArray();

            EditorUtility.SetDirty(manifest);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // build manifest
            var manifestBundleName = "manifest" + App.AssetBundleExtension;
            builds = new[] {
                new AssetBundleBuild {
                    assetNames = new[] { AssetDatabase.GetAssetPath (manifest), },
                    assetBundleName = manifestBundleName
                }
            };

            BuildPipeline.BuildAssetBundles(App.BundleTempDirectory, builds, options, EditorUserBuildSettings.activeBuildTarget);
            ArrayUtility.Add(ref assetBundles, manifestBundleName);

            // zip
            var zipfile = IOPath.PathCombine(App.DataDirectory, "res.zip");
            IOPath.FileDelete(zipfile);
            IOPath.DirectoryDelete(App.BundleDirectory);
            if (app.isDevelopmentVersion)
            {
                IOPath.DirectoryCopy(App.BundleTempDirectory, App.BundleDirectory);
            }
            else
            {
                string[] files = new string[assetBundles.Length];
                string[] parents = new string[assetBundles.Length];
                for (int i = 0; i < assetBundles.Length; i++)
                {
                    files[i] = IOPath.PathCombine(App.BundleTempDirectory, assetBundles[i]);
                    parents[i] = App.BundlePlatformName;
                }
                UZip.Zip(files, parents, zipfile);
            }
            AssetDatabase.Refresh();
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
                    assetBundleName = GetBuildBundleName(bundle.bundleName)
                });
            }
            return builds.ToArray();
        }

        private T GetAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }

            return asset;
        }
    }
}
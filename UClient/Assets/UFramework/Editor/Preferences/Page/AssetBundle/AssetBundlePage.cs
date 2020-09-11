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
            assets = describeObject.assets;
            AnalysisAssets();
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
            describeObject.assets = assets;
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
                    bundle.bundleName = asset.bundleName;
                    bundle.assets = new List<BundleAssetItem>();
                    bundle.bundleSize = GetBuildBundleSize(bundle);
                    bundleTracker.Add(asset.bundleName, bundle);

                    bundles.Add(bundle);
                }
                bundle.fileSize += asset.size;
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
                    assetItem.bundleName = FormatAssetBundleName(pathItem, pathItem.path);
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
                    bundle.bundleName = asset.bundleName;
                    bundle.assets = new List<BundleAssetItem>();
                    bundle.IsDependencies = true;
                    bundleTracker.Add(asset.bundleName, bundle);

                    bundles.Add(bundle);
                }
                bundle.fileSize += asset.size;
                bundle.bundleSize = GetBuildBundleSize(bundle);
                bundle.assets.Add(asset);

                // bundle name
                SetAssetBundleName(asset);
                var dependencies = AssetDatabase.GetDependencies(asset.path, true);
                foreach (var dAsset in dependencies)
                {
                    var dItem = new BundleAssetItem();
                    dItem.path = dAsset;
                    dItem.bundleName = asset.bundleName;
                    SetAssetBundleName(dItem);
                }
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
                            shaderAsset.bundleName = "shaders";

                            assetTracker.Add(asset);
                            assets.Add(shaderAsset);

                            shaderBundle.fileSize += shaderAsset.size;
                            shaderBundle.assets.Add(shaderAsset);

                            SetAssetBundleName(shaderAsset);
                        }
                    }
                }
            }
            shaderBundle.bundleSize = GetBuildBundleSize(shaderBundle);
            bundles.Add(shaderBundle);
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
        /// 设置 AssetBundle Name
        /// </summary>
        /// <param name="assetItem"></param>
        private void SetAssetBundleName(BundleAssetItem assetItem)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetItem.path);
            assetImporter.assetBundleName = assetItem.bundleName;
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
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 17:00:30
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.AssetBundle
{
    /// <summary>
    /// Preferences Page
    /// </summary>
    public class AssetBundlePreferencesPage : IPage, IPageBar
    {
        public string menuName => "AssetBundle/Preferences";

        static Preferences_AssetBundle_SearchPathConfig SearchPathConfig => Serializer<Preferences_AssetBundle_SearchPathConfig>.Instance;

        #region search pattern

        [BoxGroup("Pattern")] public string patternAsset = "*.asset";

        [BoxGroup("Pattern")] public string patternController = "*.controller";

        [BoxGroup("Pattern")] public string patternDir = "*";

        [BoxGroup("Pattern")] public string patternMaterial = "*.mat";

        [BoxGroup("Pattern")] public string patternTexture = "*.png";

        [BoxGroup("Pattern")] public string patternPrefab = "*.prefab";

        [BoxGroup("Pattern")] public string patternScene = "*.unity";

        [BoxGroup("Pattern")] public string patternText = "*.txt,*.bytes,*.json,*.csv,*.xml,*htm,*.html,*.yaml,*.fnt";

        #endregion

        /// <summary>
        /// 资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundlePathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector] [TableList(AlwaysExpanded = true)] [TabGroup("Custom")] [LabelText("Assets Serach Directorys")]
        public List<AssetSearchItem> assetPathItems = new List<AssetSearchItem>();

        /// <summary>
        /// 资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundlePathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector] [TableList(AlwaysExpanded = true)] [TabGroup("Custom")] [LabelText("Assets Serach File Paths")]
        public List<AssetSearchFileItem> assetFileItems = new List<AssetSearchFileItem>();

        /// <summary>
        /// 内置资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundleAssetPathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true)] [TabGroup("Built-In")] [LabelText("Assets Serach")]
        public List<AssetSearchItem> builtInAssetPathItems = new List<AssetSearchItem>();

        /// <summary>
        /// 内置资源文件列表
        /// </summary>
        /// <typeparam name="AssetBundlePathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true)] [TabGroup("Built-In")] [LabelText("Assets Serach File Paths")]
        public List<AssetSearchFileItem> builtInAssetFileItems = new List<AssetSearchFileItem>();


        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            if (!IOPath.FileExists(AssetManifest.AssetPath))
            {
                EditorUtility.SetDirty(Editor.Utils.GetAsset<AssetManifest>(AssetManifest.AssetPath));
                AssetDatabase.SaveAssets();
            }

            assetPathItems = SearchPathConfig.assetPathItems;
            assetFileItems = SearchPathConfig.assetFileItems;
            BuildBuiltInPathItems();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Optimize")))
            {
                OptimizePaths();
                OptimizeFiles();
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Remove AssetBundle Name")))
            {
                RemoveAssetBundleName();
            }
        }

        public void OnSaveDescribe()
        {
            if (SearchPathConfig == null) return;

            SearchPathConfig.assetPathItems = assetPathItems;
            SearchPathConfig.assetFileItems = assetFileItems;
            SearchPathConfig.builtInAssetPathItems = builtInAssetPathItems;
            SearchPathConfig.builtInAssetFileItems = builtInAssetFileItems;
            SearchPathConfig.Serialize();
        }

        /// <summary>
        /// optimize path
        /// </summary>
        private void OptimizePaths()
        {
            // 去重/路径是否
            var removeEmptyIndexs = new List<int>();
            var removeDuplicateIndexs = new List<int>();
            var removeNoExistsIndexs = new List<int>();
            var paths = new HashSet<string>();
            for (var i = 0; i < assetPathItems.Count; i++)
            {
                var item = assetPathItems[i];
                // pattern
                if (string.IsNullOrEmpty(item.pattern))
                {
                    item.pattern = "*";
                }

                if (string.IsNullOrEmpty(item.path))
                {
                    // 路径为空
                    removeEmptyIndexs.Add(i);
                }
                else
                {
                    if (!Directory.Exists(item.path))
                    {
                        // 路径不存在
                        removeNoExistsIndexs.Add(i);
                    }
                    else
                    {
                        if (!paths.Contains(item.path))
                        {
                            paths.Add(item.path);
                        }
                        else
                        {
                            // 重复
                            removeDuplicateIndexs.Add(i);
                        }
                    }
                }
            }

            // 移除空路径
            for (var i = removeEmptyIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeEmptyIndexs[i];
                EditorUtility.DisplayProgressBar("optimize empty", assetPathItems[index].path, (i + 1) / removeEmptyIndexs.Count);
                assetPathItems.RemoveAt(index);
            }

            // 重复路径
            for (var i = removeDuplicateIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeDuplicateIndexs[i];
                EditorUtility.DisplayProgressBar("optimize duplicate", assetPathItems[index].path, (i + 1) / removeDuplicateIndexs.Count);
                assetPathItems.RemoveAt(index);
            }

            // 不存在路径
            for (var i = removeNoExistsIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeNoExistsIndexs[i];
                EditorUtility.DisplayProgressBar("optimize not exists", assetPathItems[index].path, (i + 1) / removeNoExistsIndexs.Count);
                assetPathItems.RemoveAt(index);
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// optimize file
        /// </summary>
        private void OptimizeFiles()
        {
            var removeEmptyIndexs = new List<int>();
            var removeDuplicateIndexs = new List<int>();
            var removeNoExistsIndexs = new List<int>();
            var paths = new HashSet<string>();
            for (var i = 0; i < assetFileItems.Count; i++)
            {
                var item = assetFileItems[i];
                // pattern
                if (string.IsNullOrEmpty(item.pattern))
                {
                    item.pattern = "*";
                }

                if (string.IsNullOrEmpty(item.path))
                {
                    // 路径为空
                    removeEmptyIndexs.Add(i);
                }
                else
                {
                    if (!IOPath.FileExists(item.path))
                    {
                        // 路径不存在
                        removeNoExistsIndexs.Add(i);
                    }
                    else
                    {
                        if (!paths.Contains(item.path))
                        {
                            paths.Add(item.path);
                        }
                        else
                        {
                            // 重复
                            removeDuplicateIndexs.Add(i);
                        }
                    }
                }
            }

            // 移除空路径
            for (var i = removeEmptyIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeEmptyIndexs[i];
                EditorUtility.DisplayProgressBar("optimize empty", assetFileItems[index].path, (i + 1) / removeEmptyIndexs.Count);
                assetFileItems.RemoveAt(index);
            }

            // 重复路径
            for (var i = removeDuplicateIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeDuplicateIndexs[i];
                EditorUtility.DisplayProgressBar("optimize duplicate", assetFileItems[index].path, (i + 1) / removeDuplicateIndexs.Count);
                assetFileItems.RemoveAt(index);
            }

            // 不存在路径
            for (var i = removeNoExistsIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeNoExistsIndexs[i];
                EditorUtility.DisplayProgressBar("optimize not exists", assetFileItems[index].path, (i + 1) / removeNoExistsIndexs.Count);
                assetFileItems.RemoveAt(index);
            }

            EditorUtility.ClearProgressBar();
        }

        /// <summary>
        /// 构建内部资源路径列表
        /// </summary>
        private void BuildBuiltInPathItems()
        {
            builtInAssetPathItems.Clear();

            // config
            var config = new AssetSearchItem();
            config.path = IOPath.PathRelativeAsset(IOPath.PathCombine(UApplication.AssetsDirectory, "Serializable", Core.SerializableAssigned.AssetBundle.ToString()));
            config.nameType = NameType.Path;
            config.pattern = "*.json";
            builtInAssetPathItems.Add(config);

            // language
            var languageItem = new AssetSearchItem();
            languageItem.path = IOPath.PathRelativeAsset(IOPath.PathCombine(UApplication.AssetsDirectory, "Language", "Data"));
            languageItem.nameType = NameType.Path;
            languageItem.pattern = "*.txt";
            builtInAssetPathItems.Add(languageItem);
        }

        /// <summary>
        /// Remove AssetBundle Name
        /// </summary>
        public static void RemoveAssetBundleName()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();

            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var t in bundleNames)
            {
                AssetDatabase.RemoveAssetBundleName(t, true);
            }

            AssetDatabase.Refresh();
        }
    }
}
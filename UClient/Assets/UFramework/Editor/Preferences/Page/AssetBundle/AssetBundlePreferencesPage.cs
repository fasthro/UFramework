/*
 * @Author: fasthro
 * @Date: 2020-06-29 17:00:30
 * @Description: AssetBundle
 */
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.Assets
{
    /// <summary>
    /// Preferences Page
    /// </summary>
    public class AssetBundlePreferencesPage : IPage, IPageBar
    {
        public string menuName { get { return "AssetBundle/Preferences"; } }
        static AssetBundle_AssetSearchPathConfig describeObject;

        #region search pattern
        [BoxGroup("Pattern")]
        public string patternAsset = "*.asset";

        [BoxGroup("Pattern")]
        public string patternController = "*.controller";

        [BoxGroup("Pattern")]
        public string patternDir = "*";

        [BoxGroup("Pattern")]
        public string patternMaterial = "*.mat";

        [BoxGroup("Pattern")]
        public string patternTexture = "*.png";

        [BoxGroup("Pattern")]
        public string patternPrefab = "*.prefab";

        [BoxGroup("Pattern")]
        public string patternScene = "*.unity";

        [BoxGroup("Pattern")]
        public string patternText = "*.txt,*.bytes,*.json,*.csv,*.xml,*htm,*.html,*.yaml,*.fnt";

        #endregion

        /// <summary>
        /// 资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundlePathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Custom")]
        [LabelText("Assets Serach Directorys")]
        public List<AssetSearchItem> assetPathItems = new List<AssetSearchItem>();

        /// <summary>
        /// 资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundlePathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Custom")]
        [LabelText("Assets Serach File Paths")]
        public List<AssetSearchFileItem> assetFileItems = new List<AssetSearchFileItem>();

        /// <summary>
        /// 内置资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundleAssetPathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Built-In")]
        [LabelText("Assets Serach")]
        [ReadOnly]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<AssetSearchItem> builtInAssetPathItems = new List<AssetSearchItem>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AssetBundle_AssetSearchPathConfig>();
            assetPathItems = describeObject.assetPathItems;
            assetFileItems = describeObject.assetFileItems;
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
            describeObject.assetPathItems = assetPathItems;
            describeObject.assetFileItems = assetFileItems;
            describeObject.builtInAssetPathItems = builtInAssetPathItems;
            describeObject.Save();
        }

        /// <summary>
        /// optimize path
        /// </summary>
        private void OptimizePaths()
        {
            // 去重/路径是否
            List<int> removeEmptyIndexs = new List<int>();
            List<int> removeDuplicateIndexs = new List<int>();
            List<int> removeNoExistsIndexs = new List<int>();
            HashSet<string> paths = new HashSet<string>();
            for (int i = 0; i < assetPathItems.Count; i++)
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
            for (int i = removeEmptyIndexs.Count - 1; i >= 0; i--)
            {
                int index = removeEmptyIndexs[i];
                EditorUtility.DisplayProgressBar("optimize empty", assetPathItems[index].path, (i + 1) / removeEmptyIndexs.Count);
                assetPathItems.RemoveAt(index);
            }
            // 重复路径
            for (int i = removeDuplicateIndexs.Count - 1; i >= 0; i--)
            {
                int index = removeDuplicateIndexs[i];
                EditorUtility.DisplayProgressBar("optimize duplicate", assetPathItems[index].path, (i + 1) / removeDuplicateIndexs.Count);
                assetPathItems.RemoveAt(index);
            }
            // 不存在路径
            for (int i = removeNoExistsIndexs.Count - 1; i >= 0; i--)
            {
                int index = removeNoExistsIndexs[i];
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
            List<int> removeEmptyIndexs = new List<int>();
            List<int> removeDuplicateIndexs = new List<int>();
            List<int> removeNoExistsIndexs = new List<int>();
            HashSet<string> paths = new HashSet<string>();
            for (int i = 0; i < assetFileItems.Count; i++)
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
            for (int i = removeEmptyIndexs.Count - 1; i >= 0; i--)
            {
                int index = removeEmptyIndexs[i];
                EditorUtility.DisplayProgressBar("optimize empty", assetFileItems[index].path, (i + 1) / removeEmptyIndexs.Count);
                assetFileItems.RemoveAt(index);
            }
            // 重复路径
            for (int i = removeDuplicateIndexs.Count - 1; i >= 0; i--)
            {
                int index = removeDuplicateIndexs[i];
                EditorUtility.DisplayProgressBar("optimize duplicate", assetFileItems[index].path, (i + 1) / removeDuplicateIndexs.Count);
                assetFileItems.RemoveAt(index);
            }
            // 不存在路径
            for (int i = removeNoExistsIndexs.Count - 1; i >= 0; i--)
            {
                int index = removeNoExistsIndexs[i];
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
            config.path = IOPath.PathRelativeAsset(App.ConfigDataDirectory);
            config.nameType = NameType.Path;
            config.pattern = "*.json";
            builtInAssetPathItems.Add(config);

            // language
            var languageItem = new AssetSearchItem();
            languageItem.path = IOPath.PathRelativeAsset(App.LanguageDataDirectory);
            languageItem.nameType = NameType.Path;
            languageItem.pattern = "*.txt";
            builtInAssetPathItems.Add(languageItem);

            // table
            var tableItem = new AssetSearchItem();
            tableItem.path = IOPath.PathRelativeAsset(App.TableDataDirectory);
            tableItem.nameType = NameType.Path;
            tableItem.pattern = "*.csv";

            builtInAssetPathItems.Add(tableItem);
        }

        /// <summary>
        /// Remove AssetBundle Name
        /// </summary>
        public static void RemoveAssetBundleName()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();

            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < bundleNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(bundleNames[i], true);
            }
            AssetDatabase.Refresh();
        }
    }
}
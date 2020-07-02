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

namespace UFramework.FrameworkWindow
{
    /// <summary>
    /// Preferences Page
    /// </summary>
    public class AssetBundlePreferencesPage : IPage
    {
        public string menuName { get { return "AssetBundle/Preferences"; } }
        static AssetBundleAssetPathItemConfig describeObject;

        /// <summary>
        /// 资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundlePathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Custom")]
        public List<AssetBundleAssetPathItem> assetPathItems = new List<AssetBundleAssetPathItem>();

        /// <summary>
        /// 内置资源路径列表
        /// </summary>
        /// <typeparam name="AssetBundleAssetPathItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [TabGroup("Built-In")]
        [ReadOnly]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<AssetBundleAssetPathItem> builtInAssetPathItems = new List<AssetBundleAssetPathItem>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AssetBundleAssetPathItemConfig>();
            assetPathItems = describeObject.assetPathItems;
            BuildBuiltInPathItems();
        }

        public void OnDrawFunctoinButton()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Validation")))
            {
                ValidationPathItem();
            }
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Delect AssetBundle Name")))
            {
                DelectAssetAssetBundleNames();
            }
        }

        public void OnApply()
        {
            describeObject.assetPathItems = assetPathItems;
            describeObject.builtInAssetPathItems = builtInAssetPathItems;
            describeObject.Save();
        }

        /// <summary>
        /// 验证 PathItem
        /// </summary>
        private void ValidationPathItem()
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
                    item.pattern = "*.*";
                }

                if (string.IsNullOrEmpty(item.path))
                {
                    // 路径为空
                    removeEmptyIndexs.Add(i);
                }
                else
                {
                    if (!IOPath.FileExists(item.path) && !Directory.Exists(item.path))
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
                assetPathItems.RemoveAt(removeEmptyIndexs[i]);
            }
            // 提示错误路径
            StringBuilder builder = new StringBuilder();
            // 重复路径
            if (removeDuplicateIndexs.Count > 0)
            {
                builder.AppendLine("Duplicate Error:");
            }
            for (int i = removeDuplicateIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeDuplicateIndexs[i];
                builder.AppendLine("  ->" + assetPathItems[index].path);
                assetPathItems.RemoveAt(index);
            }
            // 不存在路径
            if (removeNoExistsIndexs.Count > 0)
            {
                builder.AppendLine("No Exists Error:");
            }
            for (int i = removeNoExistsIndexs.Count - 1; i >= 0; i--)
            {
                var index = removeNoExistsIndexs[i];
                builder.AppendLine("  ->" + assetPathItems[index].path);
                assetPathItems.RemoveAt(index);
            }
            if (builder.Length > 0)
            {
                EditorUtility.DisplayDialog("Notice", builder.ToString(), "Sure");
            }
            else
            {
                EditorUtility.DisplayDialog("Notice", "Validation Succeed!", "Sure");
            }
        }

        /// <summary>
        /// 构建内部资源路径列表
        /// </summary>
        private void BuildBuiltInPathItems()
        {
            builtInAssetPathItems.Clear();
            // config

            // language
            var languageItem = new AssetBundleAssetPathItem();
            languageItem.path = IOPath.PathRelativeAsset(App.LanguageDataDirectory);
            languageItem.buildType = AssetBundleBuildPathType.DirectoryFile;
            languageItem.assetType = AssetBundleBuildAssetType.File;
            languageItem.pattern = "*.txt";

            builtInAssetPathItems.Add(languageItem);

            // table
            var tableItem = new AssetBundleAssetPathItem();
            tableItem.path = IOPath.PathRelativeAsset(App.TableDataDirectory);
            tableItem.buildType = AssetBundleBuildPathType.DirectoryFile;
            tableItem.assetType = AssetBundleBuildAssetType.File;
            tableItem.pattern = "*.*";

            builtInAssetPathItems.Add(tableItem);
        }

        /// <summary>
        /// 删除所有资源 AseetBundle Name
        /// </summary>
        private void DelectAssetAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();

            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < bundleNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(bundleNames[i], true);
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
}
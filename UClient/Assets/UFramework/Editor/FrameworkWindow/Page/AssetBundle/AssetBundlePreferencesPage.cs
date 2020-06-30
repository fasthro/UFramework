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
        public List<AssetBundleAssetPathItem> assetPathItems = new List<AssetBundleAssetPathItem>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AssetBundleAssetPathItemConfig>();
            assetPathItems = describeObject.assetPathItems;
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
        /// 删除所有资源 AseetBundle Name
        /// </summary>
        private void DelectAssetAssetBundleNames()
        {
            var directorys = Directory.GetDirectories(Application.dataPath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < directorys.Length; i++)
            {
                var files = Directory.GetFiles(directorys[i]);
                for (int k = 0; k < files.Length; k++)
                {
                    if (files[k].EndsWith(".meta")) continue;
                    if (files[k].EndsWith(".cs")) continue;
                    if (files[k].EndsWith(".lua")) continue;
                    if (files[k].EndsWith(".unitypackage")) continue;
                    if (files[k].EndsWith(".dll")) continue;
                    if (files[k].EndsWith(".a")) continue;
                    if (files[k].EndsWith(".so")) continue;
                    if (files[k].EndsWith(".plist")) continue;
                    if (files[k].EndsWith(".bat")) continue;

                    var filePath = files[k].Replace(Application.dataPath, "Assets");
                    AssetImporter assetImporter = AssetImporter.GetAtPath(filePath);
                    assetImporter.assetBundleName = "";

                }
            }
        }
    }
}
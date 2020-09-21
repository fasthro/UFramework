/*
 * @Author: fasthro
 * @Date: 2020-07-04 09:55:51
 * @Description: AssetBundle Runtime Page (AssetBundle 运行时内存泄漏查询分析窗口)
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Asset;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace UFramework.Editor.Runtime
{
    /// <summary>
    /// AssetBundle Res
    /// </summary>
    [System.Serializable]
    public class AssetBundleRuntimeRes
    {

        /// <summary>
        /// AssetnBundle Name
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public string assetBundleName;

        /// <summary>
        /// 引用次数
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(50)]
        [ReadOnly]
        public int refCount;

        /// <summary>
        /// 内存占用大小
        /// </summary>
        [HideInInspector]
        public long size;

        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(50)]
        [ReadOnly]
        public string sizeString
        {
            get
            {
                if (size == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(size);
            }
        }
    }


    /// <summary>
    /// Asset Res
    /// </summary>
    [System.Serializable]
    public class AssetRuntimeRes
    {
        /// <summary>
        /// AssetnBundle Name
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public string assetName;

        /// <summary>
        /// AssetnBundle Name
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public string assetBundleName;

        /// <summary>
        /// 引用次数
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(50)]
        [ReadOnly]
        public int refCount;

        /// <summary>
        /// 内存占用大小
        /// </summary>
        [HideInInspector]
        public long size;

        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(150)]
        [ReadOnly]
        public string sizeString
        {
            get
            {
                if (size == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(size);
            }
        }
    }

    /// <summary>
    /// AssetBundle Res
    /// </summary>
    [System.Serializable]
    public class ResourceAssetRuntimeRes
    {

        /// <summary>
        /// Asset Name
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public string assetName;

        /// <summary>
        /// 引用次数
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(50)]
        [ReadOnly]
        public int refCount;

        /// <summary>
        /// 内存占用大小
        /// </summary>
        [HideInInspector]
        public long size;

        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(150)]
        [ReadOnly]
        public string sizeString
        {
            get
            {
                if (size == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(size);
            }
        }
    }

    /// <summary>
    /// AssetBundle Tab
    /// </summary>
    [System.Serializable]
    public class AssetBundleRuntimeTable
    {
        [ShowInInspector]
        [TabGroup("Assets")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "OnAssetTitleBarGUI")]
        public List<AssetRuntimeRes> assets = new List<AssetRuntimeRes>();

        [ShowInInspector]
        [TabGroup("Bundles")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "OnAssetBundleTitleBarGUI")]
        public List<AssetBundleRuntimeRes> assetBundles = new List<AssetBundleRuntimeRes>();

        [HideInInspector]
        public long totalAssetSize { get; set; }

        [HideInInspector]
        public string totalAssetSizeString { get; set; }

        [HideInInspector]
        public long totalAssetBundleSize { get; set; }

        [HideInInspector]
        public string totalAssetBundleSizeString { get; set; }

        private void OnAssetTitleBarGUI()
        {
            if (totalAssetSize > 0)
            {
                GUILayout.Label("totalSize: " + totalAssetSizeString);
            }
        }

        private void OnAssetBundleTitleBarGUI()
        {
            if (totalAssetBundleSize > 0)
            {
                GUILayout.Label("totalSize: " + totalAssetBundleSizeString);
            }
        }
    }

    /// <summary>
    /// Page
    /// </summary>
    public class AssetPage : IPage, IPageBar
    {
        public string menuName { get { return "Res Loader"; } }

        [ShowInInspector]
        [TabGroup("AssetBundle")]
        [HideLabel]
        public AssetBundleRuntimeTable assetBundleRuntimeTable = new AssetBundleRuntimeTable();

        [ShowInInspector]
        [TabGroup("Resource Assets")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "OnResourceTitleBarGUI")]
        public List<ResourceAssetRuntimeRes> resourceAssets = new List<ResourceAssetRuntimeRes>();

        #region total size

        [HideInInspector]
        public long totalResourceAssetSize { get; private set; }

        [HideInInspector]
        public string totalResourceAssetSizeString { get; private set; }

        #endregion


        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            Refresh();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
            {
                Refresh();
            }
        }

        public void OnSaveDescribe()
        {

        }

        private void OnResourceTitleBarGUI()
        {
            if (totalResourceAssetSize > 0)
            {
                GUILayout.Label("totalSize: " + totalResourceAssetSizeString);
            }
        }

        private void Refresh()
        {
            assetBundleRuntimeTable.totalAssetSize = 0;
            assetBundleRuntimeTable.totalAssetBundleSize = 0;
            totalResourceAssetSize = 0;

            assetBundleRuntimeTable.assets.Clear();
            assetBundleRuntimeTable.assetBundles.Clear();
            resourceAssets.Clear();

            // TODO
            var resDictionary = new Dictionary<string, AssetObject>();
            resDictionary.ForEach((KeyValuePair<string, AssetObject> item) =>
            {
                var name = item.Key;
                var res = item.Value;

                if (res.assetType == AssetType.Bundle)
                {
                    var bundle = new AssetBundleRuntimeRes();
                    bundle.assetBundleName = name;
                    bundle.refCount = res.refCount;
                    if (res.assetBundle != null)
                    {
                        bundle.size = Profiler.GetRuntimeMemorySizeLong(res.assetBundle);
                    }
                    assetBundleRuntimeTable.assetBundles.Add(bundle);

                    assetBundleRuntimeTable.totalAssetBundleSize += bundle.size;
                }
                else if (res.assetType == AssetType.BundleAsset)
                {
                    var asset = new AssetRuntimeRes();
                    asset.assetName = name;
                    asset.assetBundleName = res.bundleName;
                    asset.refCount = res.refCount;
                    if (res.assetObject != null)
                    {
                        asset.size = Profiler.GetRuntimeMemorySizeLong(res.assetObject);
                    }
                    assetBundleRuntimeTable.assets.Add(asset);

                    assetBundleRuntimeTable.totalAssetSize += asset.size;
                }
                else if (res.assetType == AssetType.Resource)
                {
                    var asset = new ResourceAssetRuntimeRes();
                    asset.assetName = name;
                    asset.refCount = res.refCount;
                    if (res.assetBundle != null)
                    {
                        asset.size = Profiler.GetRuntimeMemorySizeLong(res.assetObject);
                    }
                    resourceAssets.Add(asset);

                    totalResourceAssetSize += asset.size;
                }

            });

            assetBundleRuntimeTable.assets.Sort((x, y) => x.size.CompareTo(y.size));
            assetBundleRuntimeTable.assetBundles.Sort((x, y) => x.size.CompareTo(y.size));
            resourceAssets.Sort((x, y) => x.size.CompareTo(y.size));

            assetBundleRuntimeTable.totalAssetSizeString = EditorUtility.FormatBytes(assetBundleRuntimeTable.totalAssetSize);
            assetBundleRuntimeTable.totalAssetBundleSizeString = EditorUtility.FormatBytes(assetBundleRuntimeTable.totalAssetBundleSize);
            totalResourceAssetSizeString = EditorUtility.FormatBytes(totalResourceAssetSize);
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-07-04 09:55:51
 * @Description: AssetBundle Runtime Page (AssetBundle 运行时内存泄漏查询分析窗口)
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.ResLoader;
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
    /// Page
    /// </summary>
    public class ResLoaderRuntimePage : IPage
    {
        public string menuName { get { return "Res Loader"; } }

        [ShowInInspector]
        [TabGroup("AssetBundle - Asset")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "OnAssetTitleBarGUI")]
        public List<AssetRuntimeRes> assets = new List<AssetRuntimeRes>();

        [ShowInInspector]
        [TabGroup("AssetBundle")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "OnAssetBundleTitleBarGUI")]
        public List<AssetBundleRuntimeRes> assetBundles = new List<AssetBundleRuntimeRes>();

        [ShowInInspector]
        [TabGroup("Resource Asset")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, OnTitleBarGUI = "OnResourceTitleBarGUI")]
        public List<ResourceAssetRuntimeRes> resourceAssets = new List<ResourceAssetRuntimeRes>();

        #region total size
        [HideInInspector]
        public long totalAssetSize { get; private set; }

        [HideInInspector]
        private string m_totalAssetSizeString;

        [HideInInspector]
        public long totalAssetBundleSize { get; private set; }

        [HideInInspector]
        private string m_totalAssetBundleSizeString;

        [HideInInspector]
        public long totalResourceAssetSize { get; private set; }

        [HideInInspector]
        private string m_totalResourceAssetSizeString;

        #endregion


        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            Refresh();
        }

        public void OnDrawFunctoinButton()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
            {
                Refresh();
            }
        }

        private void OnAssetTitleBarGUI()
        {
            if (totalAssetSize > 0)
            {
                GUILayout.Label("totalSize: " + m_totalAssetSizeString);
            }
        }

        private void OnAssetBundleTitleBarGUI()
        {
            if (totalAssetBundleSize > 0)
            {
                GUILayout.Label("totalSize: " + m_totalAssetBundleSizeString);
            }
        }

        private void OnResourceTitleBarGUI()
        {
            if (totalResourceAssetSize > 0)
            {
                GUILayout.Label("totalSize: " + m_totalResourceAssetSizeString);
            }
        }

        private void Refresh()
        {
            totalAssetSize = 0;
            totalAssetBundleSize = 0;
            totalResourceAssetSize = 0;

            assets.Clear();
            assetBundles.Clear();
            resourceAssets.Clear();

            var resDictionary = ResPool.GetResDictionary();
            resDictionary.ForEach((KeyValuePair<string, Res> item) =>
            {
                var name = item.Key;
                var res = item.Value;

                if (res.resType == ResType.AssetBundle)
                {
                    var bundle = new AssetBundleRuntimeRes();
                    bundle.assetBundleName = name;
                    bundle.refCount = res.refCount;
                    bundle.size = Profiler.GetRuntimeMemorySizeLong(res.assetBundle);
                    assetBundles.Add(bundle);

                    totalAssetBundleSize += bundle.size;
                }
                else if (res.resType == ResType.AssetBundleAsset)
                {
                    var asset = new AssetRuntimeRes();
                    asset.assetName = name;
                    asset.assetBundleName = res.assetBundleName;
                    asset.refCount = res.refCount;
                    asset.size = Profiler.GetRuntimeMemorySizeLong(res.assetObject);
                    assets.Add(asset);

                    totalAssetSize += asset.size;
                }
                else if (res.resType == ResType.Resource)
                {
                    var asset = new ResourceAssetRuntimeRes();
                    asset.assetName = name;
                    asset.refCount = res.refCount;
                    asset.size = Profiler.GetRuntimeMemorySizeLong(res.assetObject);
                    resourceAssets.Add(asset);

                    totalResourceAssetSize += asset.size;
                }

            });

            assets.Sort((x, y) => x.size.CompareTo(y.size));
            assetBundles.Sort((x, y) => x.size.CompareTo(y.size));
            resourceAssets.Sort((x, y) => x.size.CompareTo(y.size));

            m_totalAssetSizeString = EditorUtility.FormatBytes(totalAssetSize);
            m_totalAssetBundleSizeString = EditorUtility.FormatBytes(totalAssetBundleSize);
            m_totalResourceAssetSizeString = EditorUtility.FormatBytes(totalResourceAssetSize);
        }
    }
}
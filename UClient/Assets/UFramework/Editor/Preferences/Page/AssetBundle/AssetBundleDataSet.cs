/*
 * @Author: fasthro
 * @Date: 2020-06-30 11:55:50
 * @Description: 
 */
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.Assets
{
    /// <summary>
    /// AssetBundle 名称类型
    /// </summary>
    public enum NameType
    {
        // 路径命名
        // - 搜索路径
        Path,

        // 文件夹命名
        // - 搜索路径下层文件夹
        Directory,

        // 上层文件夹命名
        // - 搜索路径文件夹
        TopDirectory,
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        Title,          // 按照标题字母
        FileSize,       // 按照文件尺寸
        BundleSize,     // 按照bundle尺寸
    }

    /// <summary>
    /// sort ruler
    /// </summary>
    public class SortRuler
    {
        private SortType _sortType = SortType.Title;

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType sortType
        {
            get { return _sortType; }
            set
            {
                _sortType = value;
                Sort();
            }
        }

        /// <summary>
        /// 是否为升序
        /// </summary>
        public bool ascendingOrder = true;

        private bool _ascendingOrderActive = true;

        /// <summary>
        /// 升序激活
        /// </summary>
        public bool ascendingOrderActive
        {
            get { return _ascendingOrderActive; }
            set
            {
                _ascendingOrderActive = value;
                if (value)
                {
                    if (!ascendingOrder)
                        Sort();
                    descendingOrderActive = false;
                    ascendingOrder = true;
                }
            }
        }

        private bool _descendingOrderActive = false;

        /// <summary>
        /// 降序激活
        /// </summary>
        public bool descendingOrderActive
        {
            get { return _ascendingOrderActive; }
            set
            {
                _ascendingOrderActive = value;
                if (value)
                {
                    if (ascendingOrder)
                        Sort();
                    ascendingOrderActive = false;
                    ascendingOrder = false;
                }
            }
        }

        private List<BundleItem> bundleItems;
        private List<BundleAssetItem> bundleAssetItems;

        public SortRuler(List<BundleItem> items)
        {
            bundleItems = items;
        }

        public SortRuler(List<BundleAssetItem> items)
        {
            bundleAssetItems = items;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort()
        {
            if (bundleItems != null) SortBundleItems();
            else if (bundleAssetItems != null) SortBundleAssetItems();
        }

        /// <summary>
        /// sort bundle items
        /// </summary>
        private void SortBundleItems()
        {
            if (sortType == SortType.Title)
            {
                if (ascendingOrder)
                    bundleItems.Sort((a, b) => string.Compare(a.bundleName, b.bundleName, StringComparison.Ordinal));
                else
                    bundleItems.Sort((a, b) => string.Compare(b.bundleName, a.bundleName, StringComparison.Ordinal));
            }
            else if (sortType == SortType.FileSize)
            {
                if (ascendingOrder)
                    bundleItems.Sort((x, y) => x.fileSize.CompareTo(y.fileSize));
                else
                    bundleItems.Sort((x, y) => y.fileSize.CompareTo(x.fileSize));
            }
            else if (sortType == SortType.BundleSize)
            {
                if (ascendingOrder)
                    bundleItems.Sort((x, y) => x.bundleSize.CompareTo(y.bundleSize));
                else
                    bundleItems.Sort((x, y) => y.bundleSize.CompareTo(x.bundleSize));
            }
        }

        /// <summary>
        /// sort bundle asset items
        /// </summary>
        public void SortBundleAssetItems()
        {
            if (sortType == SortType.Title)
            {
                if (ascendingOrder)
                    bundleAssetItems.Sort((a, b) => string.Compare(a.bundleName, b.bundleName, StringComparison.Ordinal));
                else
                    bundleAssetItems.Sort((a, b) => string.Compare(b.bundleName, a.bundleName, StringComparison.Ordinal));
            }
            else if (sortType == SortType.FileSize)
            {
                if (ascendingOrder)
                    bundleAssetItems.Sort((x, y) => x.size.CompareTo(y.size));
                else
                    bundleAssetItems.Sort((x, y) => y.size.CompareTo(x.size));
            }
        }
    }

    /// <summary>
    /// search path item
    /// </summary>
    [System.Serializable]
    public class AssetSearchItem
    {
        /// <summary>
        /// 目录
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ValidateInput("ValidateInputPath", "$validateInputPathMsg", InfoMessageType.Error)]
        [FolderPath]
        public string path;

        /// <summary>
        /// Name类型
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(120f)]
        public NameType nameType;

        /// <summary>
        /// 匹配模式
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(100f)]
        public string pattern = "*";

        #region 路径验证
        private string validateInputPathMsg;
        private bool ValidateInputPath(string value)
        {
            if (string.IsNullOrEmpty(value) || !IOPath.DirectoryExists(value))
            {
                validateInputPathMsg = "error: directory not exists.";
                return false;
            }
            return true;
        }
        #endregion
    }

    /// <summary>
    /// search file item
    /// </summary>
    [System.Serializable]
    public class AssetSearchFileItem
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ValidateInput("ValidateInputPath", "$validateInputPathMsg", InfoMessageType.Error)]
        [FilePath]
        public string path;

        /// <summary>
        /// Name类型
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(120f)]
        [ReadOnly]
        public NameType nameType = NameType.Path;

        /// <summary>
        /// 匹配模式
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(100f)]
        [ReadOnly]
        public string pattern = "*";

        #region 路径验证
        private string validateInputPathMsg;
        private bool ValidateInputPath(string value)
        {
            if (string.IsNullOrEmpty(value) || !IOPath.FileExists(value))
            {
                validateInputPathMsg = "error: file path not exists.";
                return false;
            }
            return true;
        }
        #endregion
    }

    /// <summary>
    /// asset bundle item
    /// </summary>
    [System.Serializable]
    public class BundleAssetItem
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public string path;

        /// <summary>
        /// display bundle name
        /// </summary>
        [HideInInspector]
        public string bundleName;

        /// <summary>
        /// 资源大小
        /// </summary>
        [HideInInspector]
        public long size;

        /// <summary>
        /// 资源大小文本
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(70f)]
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

        /// <summary>
        /// 是否为依赖资源
        /// </summary>
        [HideInInspector]
        public bool IsDependencies = false;
    }

    [System.Serializable]
    public class BundleItem
    {
        /// <summary>
        /// display bundle name
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public string bundleName;

        /// <summary>
        /// 资源
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public List<BundleAssetItem> assets;

        /// <summary>
        /// 资源总大小
        /// </summary>
        [HideInInspector]
        public long fileSize;

        /// <summary>
        /// 资源总大小文本
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(70f)]
        [ReadOnly]
        public string fileSizeString
        {
            get
            {
                if (fileSize == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(fileSize);
            }
        }

        /// <summary>
        /// assetBundle大小
        /// </summary>
        [HideInInspector]
        public long bundleSize;

        /// <summary>
        /// assetBundle大小文本
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup(70f)]
        [ReadOnly]
        public string bundleSizeString
        {
            get
            {
                if (bundleSize == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(bundleSize);
            }
        }

        /// <summary>
        /// 是否为依赖bundle
        /// </summary>
        [HideInInspector]
        public bool IsDependencies = false;

        /// <summary>
        /// 获取资源路径列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetAssetPaths()
        {
            List<string> paths = new List<string>();
            foreach (var item in assets)
            {
                paths.Add(item.path);
            }
            return paths;
        }
    }


    /// <summary>
    /// asset search path config
    /// </summary>
    public class AssetBundle_AssetSearchPathConfig : IConfigObject
    {
        public string name { get { return "AssetBundle_AssetSearchPathConfig"; } }
        public FileAddress address { get { return FileAddress.Editor; } }

        public List<AssetSearchItem> assetPathItems = new List<AssetSearchItem>();
        public List<AssetSearchFileItem> assetFileItems = new List<AssetSearchFileItem>();
        public List<AssetSearchItem> builtInAssetPathItems = new List<AssetSearchItem>();

        public void Save()
        {
            UConfig.Write<AssetBundle_AssetSearchPathConfig>(this);
        }
    }

    /// <summary>
    /// assets config
    /// </summary>
    public class AssetBundle_AssetConfig : IConfigObject
    {
        public string name { get { return "AssetBundle_AssetConfig"; } }
        public FileAddress address { get { return FileAddress.Editor; } }

        public List<BundleAssetItem> assets = new List<BundleAssetItem>();

        public void Save()
        {
            UConfig.Write<AssetBundle_AssetConfig>(this);
        }
    }
}
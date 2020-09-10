/*
 * @Author: fasthro
 * @Date: 2020-06-30 11:55:50
 * @Description: 
 */
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
        TopDirectory
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        Title,    // 按照标题字母
        Size,     // 按照文件尺寸
    }

    /// <summary>
    /// search path item
    /// </summary>
    [System.Serializable]
    public class AssetSearchItem
    {
        /// <summary>
        /// 目录/文件
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
            if (string.IsNullOrEmpty(value) || (!IOPath.FileExists(value) && !IOPath.DirectoryExists(value)))
            {
                validateInputPathMsg = "error: path not exists.";
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
        public string displayBundleName;

        /// <summary>
        /// bundle Name(采用 md5)
        /// </summary>
        /// <value></value>
        public string bundleName { get { return UHash.GetMD5Hash(displayBundleName) + App.AssetBundleSuffix; } }

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
        [HorizontalGroup(50f)]
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
        public string displayBundleName;

        /// <summary>
        /// bundle Name(采用 md5)
        /// </summary>
        /// <value></value>
        public string bundleName { get { return UHash.GetMD5Hash(displayBundleName) + App.AssetBundleSuffix; } }

        /// <summary>
        /// 资源
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ReadOnly]
        public List<BundleAssetItem> assets;

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
        [HorizontalGroup(50f)]
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
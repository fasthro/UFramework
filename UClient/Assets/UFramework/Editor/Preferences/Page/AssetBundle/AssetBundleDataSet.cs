// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-30 11:55:50
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UFramework.Core;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.AssetBundle
{
    /// <summary>
    /// AssetBundle 名称类型
    /// </summary>
    public enum NameType
    {
        Path,
        MultipleDirectory,
        Directory,
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        Title, // 按照标题字母
        FileSize, // 按照文件尺寸
        BundleSize, // 按照bundle尺寸
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
            get => _sortType;
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
            get => _ascendingOrderActive;
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
            get => _ascendingOrderActive;
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
        [ShowInInspector, HideLabel, FolderPath] [HorizontalGroup("Search Path")] [ValidateInput("ValidateInputPath", "$_validateInputPathMsg", InfoMessageType.Error)]
        public string path;

        /// <summary>
        /// Name类型
        /// </summary>
        [ShowInInspector, HideLabel] [HorizontalGroup("Name Type")] [TableColumnWidth(140, false)]
        public NameType nameType;

        /// <summary>
        /// 匹配模式
        /// </summary>
        [ShowInInspector, HideLabel] [HorizontalGroup("Pattern")] [TableColumnWidth(150, false)]
        public string pattern = "*";

        #region 路径验证

        private string _validateInputPathMsg;

        private bool ValidateInputPath(string value)
        {
            if (string.IsNullOrEmpty(value) || !IOPath.DirectoryExists(value))
            {
                _validateInputPathMsg = "error: directory not exists.";
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
        [ShowInInspector, HideLabel, FilePath] [HorizontalGroup("File Path")] [ValidateInput("ValidateInputPath", "$_validateInputPathMsg", InfoMessageType.Error)]
        public string path;

        /// <summary>
        /// Name类型
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly] [HorizontalGroup("Name Type")] [TableColumnWidth(140, false)]
        public NameType nameType = NameType.Path;

        /// <summary>
        /// 匹配模式
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly] [HorizontalGroup("Pattern")] [TableColumnWidth(150, false)]
        public string pattern = "*";

        #region 路径验证

        private string _validateInputPathMsg;

        private bool ValidateInputPath(string value)
        {
            if (string.IsNullOrEmpty(value) || !IOPath.FileExists(value))
            {
                _validateInputPathMsg = "error: file path not exists.";
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
        [ShowInInspector, HideLabel, HorizontalGroup, ReadOnly]
        public string path;

        /// <summary>
        /// 目标对象
        /// </summary>
        [ShowInInspector, HideLabel] [HorizontalGroup(200)]
        private UnityEngine.Object target;

        public UnityEngine.Object GetTarget()
        {
            return target;
        }

        /// <summary>
        /// display bundle name
        /// </summary>
        [HideInInspector] public string bundleName;

        /// <summary>
        /// 资源大小
        /// </summary>
        [HideInInspector] public long size;

        /// <summary>
        /// 资源大小文本
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup(70f)]
        public string sizeString => size == 0 ? "--" : EditorUtility.FormatBytes(size);

        /// <summary>
        /// 是否为依赖资源
        /// </summary>
        [HideInInspector] public bool IsDependencies = false;

        /// <summary>
        /// 是否为Prefab资源
        /// </summary>
        /// <returns></returns>
        [HideInInspector]
        public bool isPrefab => Path.GetExtension(path).Equals(".prefab");


        /// <summary>
        /// 是否为材质球资源
        /// </summary>
        /// <returns></returns>
        [HideInInspector]
        public bool isMaterial => Path.GetExtension(path).Equals(".mat");

        /// <summary>
        /// 是否为贴图资源
        /// </summary>
        /// <returns></returns>
        [HideInInspector]
        public bool isTexture
        {
            get
            {
                var extension = Path.GetExtension(path);
                return extension.Equals(".png") || extension.Equals(".tga");
            }
        }

        /// <summary>
        /// 是否为Shader资源
        /// </summary>
        /// <returns></returns>
        [HideInInspector]
        public bool isShader => Path.GetExtension(path).Equals(".shader");

        /// <summary>
        /// update
        /// </summary>
        public void Update()
        {
            // target 
            if (isPrefab) target = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            else if (isMaterial) target = AssetDatabase.LoadAssetAtPath<Material>(path);
            else if (isTexture) target = AssetDatabase.LoadAssetAtPath<Texture>(path);
            else if (isShader) target = AssetDatabase.LoadAssetAtPath<Shader>(path);
            else target = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

            // size
            size = IOPath.FileSize(path);
        }
    }

    [System.Serializable]
    public class BundleItem
    {
        /// <summary>
        /// display bundle name
        /// </summary>
        [ShowInInspector, HideLabel, HorizontalGroup, ReadOnly]
        public string bundleName;

        /// <summary>
        /// 资源
        /// </summary>
        [ShowInInspector, HideLabel, HorizontalGroup] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)] [LabelText("Assets")]
        public List<BundleAssetItem> assets;

        /// <summary>
        /// 资源总大小
        /// </summary>
        [HideInInspector] public long fileSize;

        /// <summary>
        /// 资源总大小文本
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup(70f)]
        public string fileSizeString => fileSize == 0 ? "--" : EditorUtility.FormatBytes(fileSize);

        /// <summary>
        /// assetBundle大小
        /// </summary>
        [HideInInspector] public long bundleSize;

        /// <summary>
        /// assetBundle大小文本
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup(70f)]
        public string bundleSizeString => bundleSize == 0 ? "--" : EditorUtility.FormatBytes(bundleSize);

        /// <summary>
        /// 是否为依赖bundle
        /// </summary>
        [HideInInspector] public bool IsDependencies = false;

        /// <summary>
        /// 获取资源路径列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetAssetPaths()
        {
           var paths = new List<string>();
            foreach (var item in assets)
            {
                paths.Add(item.path);
            }

            return paths;
        }
    }

    public class Preferences_AssetBundle_SearchPathConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Editor;

        public List<AssetSearchItem> assetPathItems = new List<AssetSearchItem>();
        public List<AssetSearchFileItem> assetFileItems = new List<AssetSearchFileItem>();
        public List<AssetSearchItem> builtInAssetPathItems = new List<AssetSearchItem>();
        public List<AssetSearchFileItem> builtInAssetFileItems = new List<AssetSearchFileItem>();

        public void Serialize()
        {
            Serializer<Preferences_AssetBundle_SearchPathConfig>.Serialize(this);
        }
    }

    public class Preferences_AssetBundle_AssetConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Editor;

        public List<BundleItem> bundles = new List<BundleItem>();
        public List<BundleAssetItem> assets = new List<BundleAssetItem>();
        public List<BundleAssetItem> dependencieAssets = new List<BundleAssetItem>();
        public List<BundleAssetItem> builtInAssets = new List<BundleAssetItem>();

        public void Serialize()
        {
            Serializer<Preferences_AssetBundle_AssetConfig>.Serialize(this);
        }
    }
}
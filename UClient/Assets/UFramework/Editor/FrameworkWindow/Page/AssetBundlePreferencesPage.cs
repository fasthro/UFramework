/*
 * @Author: fasthro
 * @Date: 2020-06-29 17:00:30
 * @Description: AssetBundle
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UFramework.FrameworkWindow
{
    /// <summary>
    /// AssetBundle 路径打包类型
    /// </summary>
    public enum AssetBundlePathBuildType
    {
        /// <summary>
        /// 单文件
        /// </summary>
        File,
        /// <summary>
        /// 目录内单文件
        /// </summary>
        DirectoryFile,
        /// <summary>
        /// 整目录
        /// </summary>
        Directory,
        /// <summary>
        /// 子目录单文件
        /// </summary>
        SubDirectoryFile,
        /// <summary>
        /// 整子目录
        /// </summary>
        SubDirectory,
        /// <summary>
        /// 标准资源
        /// </summary>
        Standard,
    }

    /// <summary>
    /// 标准资源类型
    /// </summary>
    public enum AssetBundleBuildStandardType
    {
        Prefab,
        Texture,
        Materail,
        Animation,
        AnimatorController,
    }

    /// <summary>
    /// AssetBundle PathItem
    /// </summary>
    public class AssetBundlePathItem
    {
        [ShowInInspector]
        public string path;

        [ShowInInspector]
        public AssetBundlePathBuildType pathBuildType;
    }

    /// <summary>
    /// Preferences Page
    /// </summary>
    public class AssetBundlePreferencesPage : IPage
    {
        public string menuName { get { return "AssetBundle/Preferences"; } }

        [ShowInInspector]
        public List<AssetBundlePathItem> resPathItems = new List<AssetBundlePathItem>();

        public object GetInstance()
        {
            return this;
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }

        public void OnRenderBefore()
        {

        }
    }
}
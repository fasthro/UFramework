/*
 * @Author: fasthro
 * @Date: 2020-09-10 17:13:37
 * @Description: Manifest
 */

using System;
using UnityEngine;

namespace UFramework.Asset
{
    [Serializable]
    public class AssetRef
    {
        public string name;
        public int bundle;
        public int directory;
    }

    [Serializable]
    public class BundleRef
    {
        public string name;
        public int id;
        public int[] dependencies;
        public long size;
        public string hash;
    }

    public class Manifest : ScriptableObject
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public readonly static string PATH = "Assets/Manifest.asset";

        /// <summary>
        /// Bundle Name
        /// </summary>
        public readonly static string BUNDLE_NAME = "manifest";

        /// <summary>
        /// 资源目录
        /// </summary>
        public string[] directorys = new string[0];

        /// <summary>
        /// assets
        /// </summary>
        public AssetRef[] assets = new AssetRef[0];

        /// <summary>
        /// bundles
        /// </summary>
        public BundleRef[] bundles = new BundleRef[0];
    }
}
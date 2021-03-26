// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-17 15:33:31
// * @Description:
// --------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace UFramework.Core
{
    [Serializable]
    public class AssetRef
    {
        public string name;
        public int bundle;
        public int dirIndex;
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

    public class AssetManifest : ScriptableObject
    {
        public static readonly string AssetPath = "Assets/AssetManifest.asset";
        public static readonly string AssetBundleFileName = "manifest" + Assets.Extension;

        /// <summary>
        /// 资源目录
        /// </summary>
        public string[] dirs = new string[0];

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
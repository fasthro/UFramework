/*
 * @Author: fasthro
 * @Date: 2020-09-10 17:13:37
 * @Description: Custom Manifest
 */

using System;
using UnityEngine;

namespace UFramework
{
    [Serializable]
    public class AssetRef
    {
        public string name;
        public int bundle;
        public int dir;
    }

    [Serializable]
    public class BundleRef
    {
        public string name;
        public int id;
        public int[] deps;
        public long len;
        public string hash;
    }

    public class Manifest : ScriptableObject
    {
        public string[] activeVariants = new string[0];
        public string[] dirs = new string[0];
        public AssetRef[] assets = new AssetRef[0];
        public BundleRef[] bundles = new BundleRef[0];
    }
}
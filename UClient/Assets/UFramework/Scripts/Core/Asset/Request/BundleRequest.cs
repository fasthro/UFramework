/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class BundleRequest : AssetRequest
    {
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        private List<BundleRequest> dependencies = new List<BundleRequest>();

        public static BundleRequest Allocate(string _url)
        {
            return ObjectPool<BundleRequest>.Instance.Allocate().Initialize(_url);
        }

        private BundleRequest Initialize(string _url)
        {
            url = _url;
            return this;
        }

        public override void Recycle()
        {
            ObjectPool<BundleRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            if (loadState != LoadState.Init) return;

            var bundles = UAsset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                dependencies.Add(UAsset.Instance.LoadBundle(bundles[i]));
            }
            asset = AssetBundle.LoadFromFile(url);
            loadState = LoadState.Loaded;
        }

        public override void Unload()
        {
            for (int i = 0; i < dependencies.Count; i++)
                dependencies[i].Unload();

            Release();
        }

        protected override void OnReferenceEmpty()
        {
            dependencies.Clear();
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                asset = null;
            }
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
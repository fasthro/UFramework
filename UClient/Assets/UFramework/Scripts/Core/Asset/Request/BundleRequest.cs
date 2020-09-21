/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Assets
{
    public class BundleRequest : AssetRequest
    {
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        private List<BundleRequest> dependencies = new List<BundleRequest>();

        public override bool isAsset { get { return false; } }

        public static BundleRequest Allocate()
        {
            return ObjectPool<BundleRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<BundleRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;

            var bundles = Asset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = Asset.Instance.GetBundle<BundleRequest>(bundles[i], false);
                bundle.Load();
                dependencies.Add(bundle);
            }
            asset = AssetBundle.LoadFromFile(url);
            loadState = LoadState.Loaded;
            OnCallback();
        }

        public override void Unload()
        {
            base.Unload();
            for (int i = 0; i < dependencies.Count; i++)
                dependencies[i].Unload();
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
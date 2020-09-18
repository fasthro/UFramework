/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle asset
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class BundleAssetRequest : AssetRequest
    {
        public BundleRequest bundle { get; private set; }

        public static BundleAssetRequest Allocate()
        {
            return ObjectPool<BundleAssetRequest>.Instance.Allocate();
        }
        
        public override void Recycle()
        {
            ObjectPool<BundleAssetRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            if (loadState != LoadState.Init) return;
            var bundleName = UAsset.Instance.GetBundleNameWithAssetName(url);
            bundle = UAsset.Instance.LoadBundle(bundleName);
            asset = bundle.assetBundle.LoadAsset(url, assetType);
            loadState = LoadState.Loaded;
        }

        public override void Unload()
        {
            Release();
        }

        protected override void OnReferenceEmpty()
        {
            if (bundle != null)
                bundle.Unload();
            bundle = null;
            asset = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
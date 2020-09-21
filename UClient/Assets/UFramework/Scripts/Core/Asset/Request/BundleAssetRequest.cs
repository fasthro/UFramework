/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle asset
 */
<<<<<<< HEAD
using UFramework.Pool;

namespace UFramework.Assets
=======
using System.Collections;
using System.Collections.Generic;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
{
    public class BundleAssetRequest : AssetRequest
    {
        public BundleRequest bundle { get; private set; }

<<<<<<< HEAD
        public override bool isAsset { get { return true; } }

=======
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
        public static BundleAssetRequest Allocate()
        {
            return ObjectPool<BundleAssetRequest>.Instance.Allocate();
        }
<<<<<<< HEAD

=======
        
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
        public override void Recycle()
        {
            ObjectPool<BundleAssetRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
<<<<<<< HEAD
            base.Load();
            if (loadState != LoadState.Init) return;

            bundle = Asset.Instance.GetBundle<BundleRequest>(Asset.Instance.GetBundleNameWithAssetName(url), false);
            bundle.Load();

            asset = bundle.assetBundle.LoadAsset(url, assetType);

            loadState = LoadState.Loaded;
            OnCallback();
=======
            if (loadState != LoadState.Init) return;
            var bundleName = UAsset.Instance.GetBundleNameWithAssetName(url);
            bundle = UAsset.Instance.LoadBundle(bundleName);
            asset = bundle.assetBundle.LoadAsset(url, assetType);
            loadState = LoadState.Loaded;
        }

        public override void Unload()
        {
            Release();
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
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
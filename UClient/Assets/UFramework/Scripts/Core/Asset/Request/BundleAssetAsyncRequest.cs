/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle asset async
 */
using System.Collections;
using UnityEngine;

namespace UFramework.Core
{
    public class BundleAssetAsyncRequest : AssetRequest
    {
        public AssetBundleRequest request { get; private set; }
        public BundleAsyncRequest bundle { get; private set; }

        public override bool isAsset { get { return true; } }

        public static BundleAssetAsyncRequest Allocate()
        {
            return ObjectPool<BundleAssetAsyncRequest>.Instance.Allocate();
        }

        public override IEnumerator DoCoroutineWork()
        {
            loadState = LoadState.LoadAsset;
            request = bundle.assetBundle.LoadAssetAsync(url, assetType);
            yield return request;
            if (loadState == LoadState.LoadAsset && request.isDone)
            {
                asset = request.asset;
                loadState = LoadState.Loaded;
                Completed();
            }
        }

        public override void Recycle()
        {
            ObjectPool<BundleAssetAsyncRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
            bundle = Assets.Instance.GetBundle<BundleAsyncRequest>(Assets.Instance.GetBundleNameWithAssetName(url), true);
            if (bundle.isDone)
                StartCoroutine();
            else
            {
                loadState = LoadState.LoadBundle;
                bundle.AddCallback(OnBundleDone).Load();
            }
        }

        private void OnBundleDone(AssetRequest request)
        {
            bundle.RemoveCallback(OnBundleDone);
            StartCoroutine();
        }

        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unload;
            if (bundle != null)
                bundle.Unload();
            bundle = null;
            asset = null;
            request = null;
            base.OnReferenceEmpty();
        }
    }
}
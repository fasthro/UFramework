/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle asset async
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class BundleAssetAsyncRequest : AssetRequest
    {
        public AssetBundleRequest request { get; private set; }
        public BundleAsyncRequest bundle { get; private set; }

        public static BundleAssetAsyncRequest Allocate()
        {
            return ObjectPool<BundleAssetAsyncRequest>.Instance.Allocate();
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            loadState = LoadState.LoadAsset;
            request = bundle.assetBundle.LoadAssetAsync(url, assetType);
            yield return request;
            if (loadState == LoadState.LoadAsset && request.isDone)
            {
                asset = request.asset;
                loadState = LoadState.Loaded;
            }
            UCoroutineTask.TaskComplete();
        }

        public override void Recycle()
        {
            ObjectPool<BundleAssetAsyncRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            if (loadState != LoadState.Init) return;
            bundle = UAsset.Instance.LoadAsyncBundle(UAsset.Instance.GetBundleNameWithAssetName(url));
            if (bundle.isDone)
                UCoroutineTask.AddTaskRunner(this);
            else
            {
                loadState = LoadState.LoadBundle;
                bundle.AddCallback(OnBundleDone);
            }
        }

        private void OnBundleDone(AssetRequest request)
        {
            UCoroutineTask.AddTaskRunner(this);
        }

        public override void Unload()
        {
            Release();
        }

        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unload;

            base.OnReferenceEmpty();
        }
    }
}
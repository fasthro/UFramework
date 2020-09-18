/*
 * @Author: fasthro
 * @Date: 2020-09-21 10:27:50
 * @Description: resource async asset
 */
using System.Collections;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Assets
{
    public class ResourceAssetAsyncRequest : AssetRequest
    {
        private ResourceRequest request;

        public override bool isAsset { get { return true; } }

        public static ResourceAssetAsyncRequest Allocate()
        {
            return ObjectPool<ResourceAssetAsyncRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<ResourceAssetAsyncRequest>.Instance.Recycle(this);
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            loadState = LoadState.LoadAsset;

            request = Resources.LoadAsync(url, assetType);
            yield return request;

            if (loadState == LoadState.LoadAsset && request.isDone)
            {
                asset = request.asset;
                loadState = LoadState.Loaded;
                OnAsyncCallback();
            }
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
            StartCoroutine();
        }

        protected override void OnReferenceEmpty()
        {
            if (asset != null)
                Resources.UnloadAsset(asset);
            asset = null;
            request = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
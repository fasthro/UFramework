// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;

namespace UFramework.Core
{
    public class ResourceAssetAsyncRequest : AssetRequest
    {
        public override bool isAsset => true;

        private ResourceRequest _request;

        public static ResourceAssetAsyncRequest Allocate()
        {
            return ObjectPool<ResourceAssetAsyncRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<ResourceAssetAsyncRequest>.Instance.Recycle(this);
        }

        public override IEnumerator DoCoroutineWork()
        {
            loadState = LoadState.LoadAsset;

            _request = Resources.LoadAsync(url, assetType);
            yield return _request;

            if (loadState == LoadState.LoadAsset && _request.isDone)
            {
                asset = _request.asset;
                loadState = LoadState.Loaded;
                Completed();
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
            _request = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
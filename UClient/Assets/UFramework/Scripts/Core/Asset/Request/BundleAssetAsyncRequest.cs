// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description: 异步请求AssetBundle内的资源
// --------------------------------------------------------------------------------

using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UFramework.Core
{
    public class BundleAssetAsyncRequest : AssetRequest
    {
        public AssetBundleRequest request { get; private set; }
        public BundleAsyncRequest bundle { get; private set; }

        public override bool isAsset => true;
        public override float progress => request?.progress ?? 0;

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

#if UNITY_EDITOR
            if (Assets.isUseAssetBundle)
            {
                asset = AssetDatabase.LoadAssetAtPath(url, assetType);
                Completed();
            }
            else
            {
#endif
                bundle = Assets.Instance.GetBundle<BundleAsyncRequest>(Assets.Instance.GetBundleNameWithAssetName(url), true);
                if (bundle.isDone)
                    StartCoroutine();
                else
                {
                    loadState = LoadState.LoadBundle;
                    bundle.AddCallback(OnBundleDone).Load();
                }
#if UNITY_EDITOR
            }
#endif
        }

        private void OnBundleDone(AssetRequest request)
        {
            bundle.RemoveCallback(OnBundleDone);
            StartCoroutine();
        }

        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unload;
            bundle?.Unload();
            bundle = null;
            asset = null;
            request = null;
            base.OnReferenceEmpty();
        }
    }
}
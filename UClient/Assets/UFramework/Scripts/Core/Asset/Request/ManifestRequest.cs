/*
 * @Author: fasthro
 * @Date: 2020-09-21 11:04:42
 * @Description: manifest
 */
using System.Collections;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Assets
{
    public class ManifestRequest : AssetRequest
    {
        public AssetBundleRequest request { get; private set; }
        public BundleAsyncRequest bundle { get; private set; }
        public AssetManifest manifest { get; private set; }

        public override bool isAsset { get { return true; } }

        public static ManifestRequest Allocate()
        {
            return ObjectPool<ManifestRequest>.Instance.Allocate();
        }

        public ManifestRequest()
        {
            name = url = AssetManifest.AssetPath;
        }

        public override void Recycle()
        {
            ObjectPool<ManifestRequest>.Instance.Recycle(this);
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            loadState = LoadState.LoadAsset;
            request = bundle.assetBundle.LoadAssetAsync(AssetManifest.AssetPath, typeof(AssetManifest));
            yield return request;
            if (loadState == LoadState.LoadAsset && request.isDone)
            {
                asset = request.asset;
                manifest = asset as AssetManifest;
                loadState = LoadState.Loaded;
            }
            OnAsyncCallback();
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
            bundle = Asset.Instance.GetBundle<BundleAsyncRequest>(AssetManifest.AssetBundleName + App.AssetBundleExtension, true);
            if (bundle.isDone)
                StartCoroutine();
            else
            {
                loadState = LoadState.LoadBundle;
                bundle.AddCallback(OnBundleDone);
                bundle.Load();
            }
        }

        private void OnBundleDone(AssetRequest request)
        {
            request.RemoveCallback(OnBundleDone);
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
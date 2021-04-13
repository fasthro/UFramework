// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections;
using UFramework.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UFramework.Core
{
    public class ManifestRequest : AssetRequest
    {
        public AssetManifest manifest { get; private set; }

        public override bool isAsset => true;
        public override float progress => _request?.progress ?? 0;

        private AssetBundleRequest _request;
        private BundleAsyncRequest _bundle;

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

        public override IEnumerator DoCoroutineWork()
        {
            loadState = LoadState.LoadAsset;
            _request = _bundle.assetBundle.LoadAssetAsync(AssetManifest.AssetPath, typeof(AssetManifest));
            yield return _request;
            if (loadState == LoadState.LoadAsset && _request.isDone)
            {
                asset = _request.asset;
                manifest = asset as AssetManifest;
                loadState = LoadState.Loaded;
            }

            Completed();
        }

        public override void Load()
        {
#if UNITY_EDITOR
            manifest = AssetDatabase.LoadAssetAtPath<AssetManifest>(AssetManifest.AssetPath);
            Completed();
#endif
            base.Load();
            if (loadState != LoadState.Init) return;
            _bundle = Assets.Instance.GetBundle<BundleAsyncRequest>(AssetManifest.AssetBundleFileName, true);
            if (_bundle.isDone)
                StartCoroutine();
            else
            {
                loadState = LoadState.LoadBundle;
                _bundle.AddCallback(OnBundleDone);
                _bundle.Load();
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
            _bundle?.Unload();
            _bundle = null;
            asset = null;
            _request = null;
            manifest = null;
            base.OnReferenceEmpty();
        }
    }
}
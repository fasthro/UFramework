/*
 * @Author: fasthro
 * @Date: 2020-09-21 11:04:42
 * @Description: manifest
 */
using System.Collections;
using UFramework.Pool;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UFramework.Assets
{
    public class ManifestRequest : AssetRequest
    {
        public AssetManifest manifest { get; private set; }
        public override bool isAsset { get { return true; } }

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

        public override IEnumerator OnCoroutineTaskRun()
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
            OnAsyncCallback();
        }

        public override void Load()
        {
            if (Asset.Develop)
            {
#if UNITY_EDITOR
                manifest = AssetDatabase.LoadAssetAtPath<AssetManifest>(AssetManifest.AssetPath);
                OnCallback();
#endif
            }
            else
            {
                base.Load();
                if (loadState != LoadState.Init) return;
                _bundle = Asset.Instance.GetBundle<BundleAsyncRequest>(AssetManifest.AssetBundleFileName, true);
                if (_bundle.isDone)
                    StartCoroutine();
                else
                {
                    loadState = LoadState.LoadBundle;
                    _bundle.AddCallback(OnBundleDone);
                    _bundle.Load();
                }
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
            if (_bundle != null)
                _bundle.Unload();
            _bundle = null;
            asset = null;
            _request = null;
            manifest = null;
            base.OnReferenceEmpty();
        }
    }
}
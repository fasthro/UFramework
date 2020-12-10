/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: async bundle
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Assets
{
    public class BundleAsyncRequest : AssetRequest
    {
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        public override bool isAsset { get { return false; } }

        private AssetBundleCreateRequest _request;
        private List<BundleAsyncRequest> _dependencies = new List<BundleAsyncRequest>();

        public static BundleAsyncRequest Allocate()
        {
            return ObjectPool<BundleAsyncRequest>.Instance.Allocate();
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            loadState = LoadState.LoadBundle;

            _request = AssetBundle.LoadFromFileAsync(url);
            yield return _request;

            if (loadState == LoadState.LoadBundle && _request.isDone)
            {
                asset = _request.assetBundle;
                loadState = LoadState.Loaded;
                OnAsyncCallback();
            }
        }

        public override void Recycle()
        {
            ObjectPool<BundleAsyncRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;

            var bundles = Asset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = Asset.Instance.GetBundle<BundleAsyncRequest>(url, true);
                bundle.AddCallback(OnBundleDone);
                bundle.Load();
                _dependencies.Add(bundle);
            }
            StartCoroutine();
        }

        private void OnBundleDone(AssetRequest request)
        {
            request.RemoveCallback(OnBundleDone);
            StartCoroutine();
        }


        public override void Unload()
        {
            base.Unload();
            for (int i = 0; i < _dependencies.Count; i++)
                _dependencies[i].Unload();
        }

        protected override void OnReferenceEmpty()
        {
            _dependencies.Clear();
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                asset = null;
            }
            _request = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
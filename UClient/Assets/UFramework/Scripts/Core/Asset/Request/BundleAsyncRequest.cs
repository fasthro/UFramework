// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description: 异步请求AssetBundle
// --------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Core
{
    public class BundleAsyncRequest : AssetRequest
    {
        public AssetBundle assetBundle => asset as AssetBundle;
        public override bool isAsset => false;
        public override float progress => _request?.progress ?? 0;

        private AssetBundleCreateRequest _request;
        private List<BundleAsyncRequest> _dependencies = new List<BundleAsyncRequest>();

        public static BundleAsyncRequest Allocate()
        {
            return ObjectPool<BundleAsyncRequest>.Instance.Allocate();
        }

        public override IEnumerator DoCoroutineWork()
        {
            loadState = LoadState.LoadBundle;

            _request = AssetBundle.LoadFromFileAsync(url);
            yield return _request;
            if (loadState == LoadState.LoadBundle && _request.isDone)
            {
                asset = _request.assetBundle;
                loadState = LoadState.Loaded;
                Completed();
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

            var bundles = Assets.Instance.GetDependencies(url);
            for (var i = 0; i < bundles.Length; i++)
            {
                var bundle = Assets.Instance.GetBundle<BundleAsyncRequest>(url, true);
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


        public override void Unload(bool unloadAllLoadedObjects)
        {
            base.Unload(unloadAllLoadedObjects);
            for (var i = 0; i < _dependencies.Count; i++)
                _dependencies[i].Unload();
        }

        protected override void OnReferenceEmpty()
        {
            _dependencies.Clear();
            if (assetBundle != null)
            {
                assetBundle.Unload(unloadAllLoadedObjects);
                asset = null;
            }
            _request = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
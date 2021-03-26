// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UFramework.Core
{
    public class WebBundleRequest : AssetRequest
    {
        public AssetBundle assetBundle => asset as AssetBundle;

        private List<WebBundleRequest> _dependencies = new List<WebBundleRequest>();
        private UnityWebRequest _request;

        public override bool isAsset => false;

        public static WebBundleRequest Allocate()
        {
            return ObjectPool<WebBundleRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<WebBundleRequest>.Instance.Recycle(this);
        }

        public override IEnumerator DoCoroutineWork()
        {
            loadState = LoadState.LoadBundle;
            _request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return _request.SendWebRequest();
            if (loadState == LoadState.LoadBundle && _request.isDone)
            {
                asset = DownloadHandlerAssetBundle.GetContent(_request);
                loadState = LoadState.Loaded;
                Completed();
            }
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;

            var bundles = Assets.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = Assets.Instance.GetBundle<WebBundleRequest>(bundles[i], true);
                bundle.AddCallback(OnBundleDone);
                bundle.Load();
                _dependencies.Add(bundle);
            }
        }

        private void OnBundleDone(AssetRequest request)
        {
            request.RemoveCallback(OnBundleDone);
            StartCoroutine();
        }

        public override void Unload(bool unloadAllLoadedObjects)
        {
            base.Unload(unloadAllLoadedObjects);
            for (int i = 0; i < _dependencies.Count; i++)
                _dependencies[i].Unload();
        }

        protected override void OnReferenceEmpty()
        {
            _dependencies.Clear();
            if (_request != null)
            {
                _request.Dispose();
                _request = null;
            }
            if (assetBundle != null)
            {
                assetBundle.Unload(unloadAllLoadedObjects);
                asset = null;
            }
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
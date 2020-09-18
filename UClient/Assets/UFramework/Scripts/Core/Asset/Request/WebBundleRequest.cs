/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: web bundle
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;
using UnityEngine.Networking;

namespace UFramework.Asset
{
    public class WebBundleRequest : AssetRequest
    {
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        private List<WebBundleRequest> dependencies = new List<WebBundleRequest>();
        private UnityWebRequest request;

        public static WebBundleRequest Allocate(string _url)
        {
            return ObjectPool<WebBundleRequest>.Instance.Allocate().Initialize(_url);
        }

        private WebBundleRequest Initialize(string _url)
        {
            url = _url;
            return this;
        }

        public override void Recycle()
        {
            ObjectPool<WebBundleRequest>.Instance.Recycle(this);
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            loadState = LoadState.LoadBundle;
            request = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return request.SendWebRequest();
            if (loadState == LoadState.LoadBundle && request.isDone)
            {
                asset = DownloadHandlerAssetBundle.GetContent(request);
                loadState = LoadState.Loaded;
            }
        }

        public override void Load()
        {
            if (loadState != LoadState.Init) return;

            var bundles = UAsset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                dependencies.Add(UAsset.Instance.LoadWebBundle(bundles[i]));
            }
            UCoroutineTask.AddTaskRunner(this);
        }

        public override void Unload()
        {
            for (int i = 0; i < dependencies.Count; i++)
                dependencies[i].Unload();

            Release();
        }

        protected override void OnReferenceEmpty()
        {
            dependencies.Clear();
            if (request != null)
            {
                request.Dispose();
                request = null;
            }
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                asset = null;
            }
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
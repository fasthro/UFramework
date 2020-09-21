/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: web bundle
 */
using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using UFramework.Coroutine;
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
using UFramework.Pool;
using UnityEngine;
using UnityEngine.Networking;

<<<<<<< HEAD
namespace UFramework.Assets
=======
namespace UFramework.Asset
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
{
    public class WebBundleRequest : AssetRequest
    {
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        private List<WebBundleRequest> dependencies = new List<WebBundleRequest>();
        private UnityWebRequest request;

<<<<<<< HEAD
        public override bool isAsset { get { return false; } }

        public static WebBundleRequest Allocate()
        {
            return ObjectPool<WebBundleRequest>.Instance.Allocate();
=======
        public static WebBundleRequest Allocate(string _url)
        {
            return ObjectPool<WebBundleRequest>.Instance.Allocate().Initialize(_url);
        }

        private WebBundleRequest Initialize(string _url)
        {
            url = _url;
            return this;
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
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
<<<<<<< HEAD
                OnAsyncCallback();
=======
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
            }
        }

        public override void Load()
        {
<<<<<<< HEAD
            base.Load();
            if (loadState != LoadState.Init) return;

            var bundles = Asset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = Asset.Instance.GetBundle<WebBundleRequest>(bundles[i], true);
                bundle.AddCallback(OnBundleDone);
                bundle.Load();
                dependencies.Add(bundle);
            }
        }

        private void OnBundleDone(AssetRequest request)
        {
            request.RemoveCallback(OnBundleDone);
            StartCoroutine();
=======
            if (loadState != LoadState.Init) return;

            var bundles = UAsset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                dependencies.Add(UAsset.Instance.LoadWebBundle(bundles[i]));
            }
            UCoroutineTask.AddTaskRunner(this);
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
        }

        public override void Unload()
        {
<<<<<<< HEAD
            base.Unload();
            for (int i = 0; i < dependencies.Count; i++)
                dependencies[i].Unload();
=======
            for (int i = 0; i < dependencies.Count; i++)
                dependencies[i].Unload();

            Release();
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
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
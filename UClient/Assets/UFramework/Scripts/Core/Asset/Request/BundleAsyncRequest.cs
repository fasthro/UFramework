/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: async bundle
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class BundleAsyncRequest : AssetRequest
    {
        private AssetBundleCreateRequest request;
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        private List<BundleAsyncRequest> dependencies = new List<BundleAsyncRequest>();

        public static BundleAsyncRequest Allocate(string url)
        {
            return ObjectPool<BundleAsyncRequest>.Instance.Allocate().Initialize(url);
        }

        private BundleAsyncRequest Initialize(string _url)
        {
            url = _url;
            return this;
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            loadState = LoadState.LoadBundle;

            request = AssetBundle.LoadFromFileAsync(url);
            yield return request;

            if (loadState == LoadState.LoadBundle && request.isDone)
            {
                asset = request.assetBundle;
                loadState = LoadState.Loaded;
            }

            UCoroutineTask.TaskComplete();
        }

        public override void Recycle()
        {
            ObjectPool<BundleAsyncRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            if (loadState != LoadState.Init) return;

            var bundles = UAsset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                dependencies.Add(UAsset.Instance.LoadAsyncBundle(url));
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
            if (assetBundle != null)
            {
                assetBundle.Unload(true);
                asset = null;
            }
            request = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
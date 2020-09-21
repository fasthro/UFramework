/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle
 */
using System.Collections;
using System.Collections.Generic;
using UFramework.Pool;
using UnityEngine;

<<<<<<< HEAD
namespace UFramework.Assets
=======
namespace UFramework.Asset
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
{
    public class BundleRequest : AssetRequest
    {
        public AssetBundle assetBundle { get { return asset as AssetBundle; } }
        private List<BundleRequest> dependencies = new List<BundleRequest>();

<<<<<<< HEAD
        public override bool isAsset { get { return false; } }

        public static BundleRequest Allocate()
        {
            return ObjectPool<BundleRequest>.Instance.Allocate();
=======
        public static BundleRequest Allocate(string _url)
        {
            return ObjectPool<BundleRequest>.Instance.Allocate().Initialize(_url);
        }

        private BundleRequest Initialize(string _url)
        {
            url = _url;
            return this;
>>>>>>> 9b10d35868572eee73829c584ae1543af5057d4f
        }

        public override void Recycle()
        {
            ObjectPool<BundleRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
<<<<<<< HEAD
            base.Load();
            if (loadState != LoadState.Init) return;

            var bundles = Asset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = Asset.Instance.GetBundle<BundleRequest>(bundles[i], false);
                bundle.Load();
                dependencies.Add(bundle);
            }
            asset = AssetBundle.LoadFromFile(url);
            loadState = LoadState.Loaded;
            OnCallback();
=======
            if (loadState != LoadState.Init) return;

            var bundles = UAsset.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                dependencies.Add(UAsset.Instance.LoadBundle(bundles[i]));
            }
            asset = AssetBundle.LoadFromFile(url);
            loadState = LoadState.Loaded;
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
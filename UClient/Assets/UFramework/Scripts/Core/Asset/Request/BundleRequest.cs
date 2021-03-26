// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description: 请求AssetBundle
// --------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Core
{
    public class BundleRequest : AssetRequest
    {
        public AssetBundle assetBundle => asset as AssetBundle;

        public override bool isAsset => false;

        private List<BundleRequest> _dependencies = new List<BundleRequest>();

        public static BundleRequest Allocate()
        {
            return ObjectPool<BundleRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<BundleRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;

            var bundles = Assets.Instance.GetDependencies(url);
            for (int i = 0; i < bundles.Length; i++)
            {
                var bundle = Assets.Instance.GetBundle<BundleRequest>(bundles[i], false);
                bundle.Load();
                _dependencies.Add(bundle);
            }

            asset = AssetBundle.LoadFromFile(url);
            loadState = LoadState.Loaded;
            Completed();
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
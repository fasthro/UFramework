/*
 * @Author: fasthro
 * @Date: 2020-09-21 10:27:50
 * @Description: resource asset
 */
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Assets
{
    public class ResourceAssetRequest : AssetRequest
    {
        public override bool isAsset { get { return true; } }

        public static ResourceAssetRequest Allocate()
        {
            return ObjectPool<ResourceAssetRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<ResourceAssetRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
            asset = Resources.Load(url, assetType);
            loadState = LoadState.Loaded;
            OnCallback();
        }

        protected override void OnReferenceEmpty()
        {
            if (asset != null)
                Resources.UnloadAsset(asset);
            asset = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
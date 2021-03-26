// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Core
{
    public class ResourceAssetRequest : AssetRequest
    {
        public override bool isAsset => true;

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
            Completed();
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
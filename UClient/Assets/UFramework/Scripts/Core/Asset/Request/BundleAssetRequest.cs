/*
 * @Author: fasthro
 * @Date: 2020-09-18 14:44:45
 * @Description: bundle asset
 */
using UFramework.Core;

namespace UFramework.Core
{
    public class BundleAssetRequest : AssetRequest
    {
        public BundleRequest bundle { get; private set; }

        public override bool isAsset { get { return true; } }

        public static BundleAssetRequest Allocate()
        {
            return ObjectPool<BundleAssetRequest>.Instance.Allocate();
        }
        public override void Recycle()
        {
            ObjectPool<BundleAssetRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;

            bundle = Assets.Instance.GetBundle<BundleRequest>(Assets.Instance.GetBundleNameWithAssetName(url), false);
            bundle.Load();

            asset = bundle.assetBundle.LoadAsset(url, assetType);

            loadState = LoadState.Loaded;
            Completed();
        }

        protected override void OnReferenceEmpty()
        {
            if (bundle != null)
                bundle.Unload();
            bundle = null;
            asset = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
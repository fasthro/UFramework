// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description: 请求AssetBundle内的资源
// --------------------------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UFramework.Core
{
    public class BundleAssetRequest : AssetRequest
    {
        public BundleRequest bundle { get; private set; }
        public override bool isAsset => true;

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
#if UNITY_EDITOR
            if (Assets.isUseAssetBundle)
            {
                asset = AssetDatabase.LoadAssetAtPath(url, assetType);
                Completed();
            }
            else
            {
#endif
                bundle = Assets.Instance.GetBundle<BundleRequest>(Assets.Instance.GetBundleNameWithAssetName(url), false);
                bundle.Load();

                asset = bundle.assetBundle.LoadAsset(url, assetType);

                loadState = LoadState.Loaded;
                Completed();
#if UNITY_EDITOR
            }
#endif
        }

        protected override void OnReferenceEmpty()
        {
            bundle?.Unload();
            bundle = null;
            asset = null;
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
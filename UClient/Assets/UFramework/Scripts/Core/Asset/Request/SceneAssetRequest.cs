/*
 * @Author: fasthro
 * @Date: 2020-09-21 11:04:42
 * @Description: scene asset
 */
using UFramework.Pool;

namespace UFramework.Assets
{
    public class SceneAssetRequest : AssetRequest
    {
        public override bool isAsset { get { return true; } }
        
        public static SceneAssetRequest Allocate()
        {
            return ObjectPool<SceneAssetRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<SceneAssetRequest>.Instance.Recycle(this);
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
            OnCallback();
        }

        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
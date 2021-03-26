// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description:
// --------------------------------------------------------------------------------


namespace UFramework.Core
{
    public class SceneAssetRequest : AssetRequest
    {
        public override bool isAsset => true;

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
            Completed();
        }

        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
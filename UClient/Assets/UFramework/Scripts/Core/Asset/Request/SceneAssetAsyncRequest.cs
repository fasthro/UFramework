// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-18 11:37:03
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections;

namespace UFramework.Core
{
    public class SceneAssetAsyncRequest : AssetRequest
    {
        public override bool isAsset => true;

        public static SceneAssetAsyncRequest Allocate()
        {
            return ObjectPool<SceneAssetAsyncRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<SceneAssetAsyncRequest>.Instance.Recycle(this);
        }

        public override IEnumerator DoCoroutineWork()
        {
            yield return null;
            Completed();
        }

        public override void Load()
        {
            base.Load();
            if (loadState != LoadState.Init) return;
        }

        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unload;
            base.OnReferenceEmpty();
        }
    }
}
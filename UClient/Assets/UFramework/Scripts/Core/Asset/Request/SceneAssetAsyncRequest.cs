/*
 * @Author: fasthro
 * @Date: 2020-09-21 11:04:42
 * @Description: scene asset async
 */
using System.Collections;
using UFramework.Pool;

namespace UFramework.Assets
{
    public class SceneAssetAsyncRequest : AssetRequest
    {
        public override bool isAsset { get { return true; } }

        public static SceneAssetAsyncRequest Allocate()
        {
            return ObjectPool<SceneAssetAsyncRequest>.Instance.Allocate();
        }

        public override void Recycle()
        {
            ObjectPool<SceneAssetAsyncRequest>.Instance.Recycle(this);
        }

        public override IEnumerator OnCoroutineTaskRun()
        {
            yield return null;
            OnAsyncCallback();
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
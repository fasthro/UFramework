/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Assetbundle Loader
 */

using UFramework.Messenger;
using UFramework.Pool;

namespace UFramework.Asset
{
    public class BundleLoader : Loader, IPoolObject
    {
        public Bundle bundle { get; protected set; }
        public BundleAsset asset { get; protected set; }

        protected bool _onlyBundle;

        /// <summary>
        /// 资源加载器
        /// </summary>
        /// <param name="asset">资源路径</param>
        /// <param name="callback">异步加载完成回到</param>
        /// <returns></returns>
        public static BundleLoader AllocateAsset(string asset, UCallback<bool, AssetObject> callback = null)
        {
            return ObjectPool<BundleLoader>.Instance.Allocate().Build(asset, UAsset.Instance.GetBundleNameWithAssetName(asset), callback);
        }

        /// <summary>
        /// 资源加载器(指定bundle)
        /// </summary>
        /// <param name="asset">资源路径</param>
        /// <param name="bundle">bundle</param>
        /// <param name="callback">异步加载完成回到</param>
        /// <returns></returns>
        public static BundleLoader AllocateAsset(string asset, string bundle, UCallback<bool, AssetObject> callback = null)
        {
            return ObjectPool<BundleLoader>.Instance.Allocate().Build(asset, bundle, callback);
        }

        /// <summary>
        /// Bundle加载器
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="callback">异步加载完成回到</param>
        /// <returns></returns>
        public static BundleLoader AllocateBundle(string bundle, UCallback<bool, AssetObject> callback = null)
        {
            return ObjectPool<BundleLoader>.Instance.Allocate().Build(bundle, null, callback);
        }

        #region IPoolObject

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<BundleLoader>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }

        #endregion

        /// <summary>
        /// 构建加载器
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="bundle"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private BundleLoader Build(string asset, string bundle, UCallback<bool, AssetObject> callback)
        {
            this.m_eventCallback = callback;
            this.bundle = UAsset.Instance.GetAsset<Bundle>(bundle, null, AssetType.Bundle);
            this._onlyBundle = string.IsNullOrEmpty(asset);
            if (!_onlyBundle)
                this.asset = UAsset.Instance.GetAsset<BundleAsset>(bundle, asset, AssetType.BundleAsset);
            return this;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            bool bundleReady = bundle.LoadSync();
            bool assetReady = _onlyBundle ? true : asset.LoadSync();
            bool ready = bundleReady && assetReady;
            m_eventCallback.InvokeGracefully(ready, asset);
            return ready;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            bundle.AddListener(OnEventCallback);
            bundle.LoadAsync();
        }

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="readly"></param>
        /// <param name="res"></param>

        protected override void OnEventCallback(bool ready, AssetObject res)
        {
            if (res.assetType == AssetType.Bundle)
            {
                bundle.RemoveListener(OnEventCallback);
                if (_onlyBundle)
                {
                    m_eventCallback.InvokeGracefully(ready, res);
                }
                else
                {
                    asset.AddListener(OnEventCallback);
                    asset.LoadAsync();
                }
            }
            else if (res.assetType == AssetType.BundleAsset)
            {
                asset.RemoveListener(OnEventCallback);
                m_eventCallback.InvokeGracefully(ready, res);
            }
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="unloadAllLoadedObjects"></param>
        public override void Unload()
        {
            if (bundle != null)
            {
                bundle.RemoveListener(OnEventCallback);
                bundle.Unload();
                bundle = null;
            }

            if (asset != null)
            {
                asset.RemoveListener(OnEventCallback);
                asset.Unload();
                asset = null;
            }
            m_eventCallback = null;
            Recycle();
        }
    }
}
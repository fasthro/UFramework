/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Assetbundle Loader
 */

using UFramework.Messenger;
using UFramework.Pool;

namespace UFramework.ResLoader
{
    public class AssetBundleLoader : ResLoader, IPoolObject
    {
        public BundleRes bundleRes { get; protected set; }
        public BundleAssetRes bundleAssetRes { get; protected set; }

        // 只加载Bundle，不加载资源
        protected bool m_singleBundleLoader;

        /// <summary>
        /// 分配资源加载器
        /// </summary>
        /// <param name="resPath">资源路径</param>
        public static AssetBundleLoader AllocateRes(string resPath)
        {
            return AllocateRes(resPath, null);
        }

        /// <summary>
        /// 分配资源加载器
        /// </summary>
        /// <param name="resPath">资源路径</param>
        /// <param name="callback">异步加载完成回到</param>
        /// <returns></returns>
        public static AssetBundleLoader AllocateRes(string resPath, UCallback<bool, Res> callback)
        {
            var mapping = AssetBundleDB.GetMappingData(resPath);
            var loader = ObjectPool<AssetBundleLoader>.Instance.Allocate();
            loader.Initialize(mapping.bundleName, mapping.assetName, callback);
            return loader;
        }

        /// <summary>
        /// 分配 AssetBundle 加载器
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public static AssetBundleLoader AllocateBundle(string bundleName)
        {
            var loader = ObjectPool<AssetBundleLoader>.Instance.Allocate();
            loader.Initialize(bundleName, null, null);
            return loader;
        }

        /// <summary>
        /// 分配 AssetBundle 加载器
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="callback">异步加载完成回到</param>
        /// <returns></returns>
        public static AssetBundleLoader AllocateBundle(string bundleName, UCallback<bool, Res> callback)
        {
            var loader = ObjectPool<AssetBundleLoader>.Instance.Allocate();
            loader.Initialize(bundleName, null, callback);
            return loader;
        }

        #region IPoolObject

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<AssetBundleLoader>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }

        #endregion
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        private void Initialize(string bundleName, string assetName, UCallback<bool, Res> callback)
        {
            bundleRes = ResPool.Allocate<BundleRes>(bundleName, ResType.AssetBundle);
            m_singleBundleLoader = string.IsNullOrEmpty(assetName);
            if (!m_singleBundleLoader)
            {
                bundleAssetRes = ResPool.Allocate<BundleAssetRes>(assetName, ResType.AssetBundleAsset);
            }
            m_eventCallback = callback;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            bool bundleReady = bundleRes.LoadSync();
            bool assetReady = m_singleBundleLoader ? true : bundleAssetRes.LoadSync();
            bool ready = bundleReady && assetReady;
            m_eventCallback.InvokeGracefully(ready, bundleAssetRes);
            return ready;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            bundleRes.AddListener(OnEventCallback);
            bundleRes.LoadAsync();
        }

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="readly"></param>
        /// <param name="res"></param>

        protected override void OnEventCallback(bool ready, Res res)
        {
            if (res.resType == ResType.AssetBundle)
            {
                bundleRes.RemoveListener(OnEventCallback);
                if (m_singleBundleLoader)
                {
                    m_eventCallback.InvokeGracefully(ready, res);
                }
                else
                {
                    bundleAssetRes.AddListener(OnEventCallback);
                    bundleAssetRes.LoadAsync();
                }
            }
            else if (res.resType == ResType.AssetBundleAsset)
            {
                bundleAssetRes.RemoveListener(OnEventCallback);
                m_eventCallback.InvokeGracefully(ready, res);
            }
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload()
        {
            if (bundleRes != null)
            {
                bundleRes.RemoveListener(OnEventCallback);
                bundleRes.Unload();
                bundleRes = null;
            }

            if (bundleAssetRes != null)
            {
                bundleAssetRes.RemoveListener(OnEventCallback);
                bundleAssetRes.Unload();
                bundleAssetRes = null;
            }
            m_eventCallback = null;
            Recycle();
        }
    }
}
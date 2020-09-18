/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Resources 资源加载器
 */

using UFramework.Messenger;
using UFramework.Pool;

namespace UFramework.Asset
{
    public class ResourceLoader : Loader, IPoolObject
    {
        public ResourceAsset resourceAssetRes { get; protected set; }

        /// <summary>
        /// 分配Resource资源加载器
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static ResourceLoader Allocate(string assetName)
        {
            return Allocate(assetName, null);
        }

        /// <summary>
        /// 分配Resource资源加载器
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="callback">异步加载完成回调</param>
        /// <returns></returns>
        public static ResourceLoader Allocate(string assetName, UCallback<bool, AssetObject> callback)
        {
            var loader = ObjectPool<ResourceLoader>.Instance.Allocate();
            loader.Initialize(assetName, callback);
            return loader;
        }

        #region IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ResourceLoader>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="callback"></param>
        private void Initialize(string asset, UCallback<bool, AssetObject> callback)
        {
            resourceAssetRes = UAsset.Instance.GetAsset<ResourceAsset>(null, asset, AssetType.Resource);
            m_eventCallback = callback;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            bool ready = resourceAssetRes.LoadSync();
            m_eventCallback.InvokeGracefully(ready, resourceAssetRes);
            return ready;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            resourceAssetRes.AddListener(OnEventCallback);
            resourceAssetRes.LoadAsync();
        }

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="readly"></param>
        /// <param name="res"></param>
        protected override void OnEventCallback(bool readly, AssetObject res)
        {
            resourceAssetRes.RemoveListener(OnEventCallback);
            m_eventCallback.InvokeGracefully(readly, res);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload()
        {
            resourceAssetRes.RemoveListener(OnEventCallback);
            resourceAssetRes.Unload();
            resourceAssetRes = null;
            m_eventCallback = null;
            Recycle();
        }
    }
}
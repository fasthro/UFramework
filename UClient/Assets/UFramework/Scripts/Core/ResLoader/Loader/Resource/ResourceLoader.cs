/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Resources 资源加载器
 */

using UFramework.Messenger;
using UFramework.Pool;

namespace UFramework.ResLoader
{
    public class ResourceLoader : ResLoader, IPoolObject
    {
        public ResourceAssetRes resourceAssetRes { get; protected set; }

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
        public static ResourceLoader Allocate(string assetName, UCallback<bool, Res> callback)
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
        /// <param name="assetName"></param>
        /// <param name="callback"></param>
        private void Initialize(string assetName, UCallback<bool, Res> callback)
        {
            resourceAssetRes = ResPool.Allocate<ResourceAssetRes>(assetName, ResType.Resource);
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
        protected override void OnEventCallback(bool readly, Res res)
        {
            resourceAssetRes.RemoveListener(OnEventCallback);
            m_eventCallback.InvokeGracefully(readly, res);
        }

       /// <summary>
       /// 卸载资源
       /// </summary>
       /// <param name="unloadAllLoadedObjects"></param>
        public override void Unload(bool unloadAllLoadedObjects = true)
        {
            resourceAssetRes.RemoveListener(OnEventCallback);
            resourceAssetRes.Unload(unloadAllLoadedObjects);
            resourceAssetRes = null;
            m_eventCallback = null;
            Recycle();
        }
    }
}
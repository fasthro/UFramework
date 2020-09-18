/*
 * @Author: fasthro
 * @Date: 2019-06-26 17:52:39
 * @Description: loader
 */
using UFramework.Messenger;

namespace UFramework.Asset
{
    public abstract class Loader
    {
        // 事件回调
        protected UCallback<bool, AssetObject> m_eventCallback;

        /// <summary>
        /// 同步加载
        /// </summary>
        public abstract bool LoadSync();

        /// <summary>
        /// 异步加载
        /// </summary>
        public abstract void LoadAsync();

        /// <summary>
        /// 卸载资源
        /// </summary>
        public abstract void Unload();

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="ready"></param>
        /// <param name="res"></param>
        protected abstract void OnEventCallback(bool ready, AssetObject res);
    }
}
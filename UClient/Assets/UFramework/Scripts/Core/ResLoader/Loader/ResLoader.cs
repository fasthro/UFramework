/*
 * @Author: fasthro
 * @Date: 2019-06-26 17:52:39
 * @Description: 加载基类
 */
using UFramework.Messenger;

namespace UFramework.ResLoader
{
    public abstract class ResLoader
    {
        // 事件回调
        protected UCallback<bool, Res> m_eventCallback;

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
        /// <param name="unloadAllLoadedObjects"></param>
        public abstract void Unload(bool unloadAllLoadedObjects = true);

        /// <summary>
        /// 接收通知
        /// </summary>
        /// <param name="ready"></param>
        /// <param name="res"></param>
        protected abstract void OnEventCallback(bool ready, Res res);
    }
}
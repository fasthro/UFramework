/*
 * @Author: fasthro
 * @Date: 2019-06-22 15:28:19
 * @Description: 资源基类
 */
using UFramework.Messenger;
using UFramework.Ref;
using UnityEngine;

namespace UFramework.ResLoader
{
    /// <summary>
    /// 资源状态
    /// </summary>
    public enum ResStatus
    {
        Waiting,          // 未加载
        Loading,          // 正在加载
        Failed,           // 加载失败
        Ready,            // 已加载
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        // AssetBundle
        AssetBundle,
        // AssetBudnle内的Asset资源
        AssetBundleAsset,
        // Resource
        Resource,
    }

    /// <summary>
    /// 资源基类
    /// </summary>
    public abstract class Res : ReferenceCountObject
    {
        public string resName { get; protected set; }
        public string assetName { get; protected set; }
        public string bundleName { get; protected set; }
        public ResStatus resStatus { get; protected set; }
        public ResType resType { get; protected set; }

        public UnityEngine.Object assetObject { get; protected set; }
        public AssetBundle assetBundle { get; protected set; }

        // 加载完成回调
        protected event UCallback<bool, Res> m_eventCallback;

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
        public abstract void Unload(bool unloadAllLoadedObjects = false);

        /// <summary>
        /// 获得资源
        /// </summary>
        public T GetAsset<T>() where T : UnityEngine.Object
        {
            if (assetObject == null)
                return null;
            return assetObject as T;
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(UCallback<bool, Res> callback)
        {
            if (callback == null) return;
            m_eventCallback += callback;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(UCallback<bool, Res> callback)
        {
            if (callback == null) return;
            if (m_eventCallback == null) return;
            m_eventCallback -= callback;
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="ready"></param>
        protected void BroadcastEvent(bool ready)
        {
            m_eventCallback.InvokeGracefully(ready, this);
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2019-06-22 15:28:19
 * @Description: 资源基类
 */
using UFramework.Messenger;
using UFramework.Ref;
using UnityEngine;

namespace UFramework.Asset
{
    /// <summary>
    /// 资源状态
    /// </summary>
    public enum AssetStatus
    {
        Init,
        Loading,
        Failed,
        Loaded,
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum AssetType
    {
        Bundle,
        BundleAsset,
        Resource,
    }

    /// <summary>
    /// 资源基类
    /// </summary>
    public abstract class AssetObject : ReferenceCountObject
    {
        public string assetName { get; protected set; }
        public string bundleName { get; protected set; }
        public AssetStatus assetStatus { get; protected set; }
        public AssetType assetType { get; protected set; }

        public UnityEngine.Object assetObject { get; protected set; }
        public AssetBundle assetBundle { get; protected set; }

        // 加载完成回调
        protected event UCallback<bool, AssetObject> m_eventCallback;

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
        public void AddListener(UCallback<bool, AssetObject> callback)
        {
            if (callback == null) return;
            m_eventCallback += callback;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveListener(UCallback<bool, AssetObject> callback)
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
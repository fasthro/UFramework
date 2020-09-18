/*
 * @Author: fasthro
 * @Date: 2020-09-18 11:37:03
 * @Description: 资源请求
 */
using System;
using System.Collections;
using UFramework.Coroutine;
using UFramework.Messenger;
using UFramework.Pool;
using UFramework.Ref;

namespace UFramework.Asset
{
    public enum LoadState
    {
        Init,
        LoadBundle,
        LoadAsset,
        Loaded,
        Unload,
    }

    public abstract class AssetRequest : ReferenceCountObject, IPoolObject, IUCoroutineTaskRunner
    {
        #region interface

        public bool isRecycled { get; set; }
        public abstract void Recycle();
        public virtual void OnRecycle() { }
        public virtual IEnumerator OnCoroutineTaskRun()
        {
            yield return null;
        }

        #endregion

        public Type assetType;
        public string url;
        public LoadState loadState { get; protected set; }
        protected event UCallback<AssetRequest> callback;

        public virtual bool isDone
        {
            get { return true; }
        }

        public virtual float progress
        {
            get { return 1; }
        }

        public virtual string error { get; protected set; }

        public string text { get; protected set; }

        public byte[] bytes { get; protected set; }

        public UnityEngine.Object asset { get; internal set; }

        public virtual void Load()
        {

        }

        public virtual void Unload()
        {

        }

        protected override void OnReferenceEmpty()
        {
            Recycle();
        }

        public void AddCallback(UCallback<AssetRequest> callback)
        {
            if (callback != null)
                this.callback += callback;
        }

        public void RemoveCallback(UCallback<AssetRequest> callback)
        {
            if (callback != null)
                this.callback -= callback;
        }
    }
}
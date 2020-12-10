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

namespace UFramework.Assets
{
    public enum LoadState
    {
        Init,
        LoadBundle,
        LoadAsset,
        Loaded,
        Unload,
    }

    public abstract class AssetRequest : ReferenceObject, IPoolObject, IUCoroutineTaskRunner
    {
        #region interface

        public bool isRecycled { get; set; }
        public abstract void Recycle();

        public virtual void OnRecycle()
        {
            loadState = LoadState.Init;
            asset = null;
            _callback = null;
            Asset.Instance.RecycleAsset(this);
        }

        public virtual IEnumerator OnCoroutineTaskRun()
        {
            yield return null;
        }

        #endregion

        public Type assetType;
        public string name;
        public string url;
        public LoadState loadState { get; protected set; }
        protected event UCallback<AssetRequest> _callback;
        public virtual bool isAsset
        {
            get { return true; }
        }

        public virtual bool isDone
        {
            get { return loadState == LoadState.Loaded; }
        }

        public virtual float progress
        {
            get { return 1; }
        }

        public virtual string error { get; protected set; }

        public string text { get; protected set; }

        public byte[] bytes { get; protected set; }

        public UnityEngine.Object asset { get; internal set; }

        protected bool _isNeedLoad
        {
            get { return loadState == LoadState.Init; }
        }

        public virtual void Load()
        {
            Retain();
            if (loadState == LoadState.Loaded)
            {
                OnCallback();
            }
        }

        public virtual void Unload()
        {
            Release();
        }



        protected override void OnReferenceEmpty()
        {
            Recycle();
        }

        protected void OnCallback()
        {
            this._callback.InvokeGracefully(this);
            this._callback = null;
        }

        protected void OnAsyncCallback()
        {
            UCoroutineTask.TaskComplete();
            this._callback.InvokeGracefully(this);
            this._callback = null;
        }

        protected void StartCoroutine()
        {
            UCoroutineTask.AddTaskRunner(this);
        }

        public AssetRequest AddCallback(UCallback<AssetRequest> callback)
        {
            if (callback != null)
                this._callback += callback;
            return this;
        }

        public AssetRequest RemoveCallback(UCallback<AssetRequest> callback)
        {
            if (callback != null)
                this._callback -= callback;
            return this;
        }

        public T GetRequest<T>() where T : AssetRequest
        {
            return this as T;
        }

        public T GetAsset<T>() where T : UnityEngine.Object
        {
            return asset as T;
        }
    }
}
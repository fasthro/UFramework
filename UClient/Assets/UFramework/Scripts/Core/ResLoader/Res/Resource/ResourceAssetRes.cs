/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Resources Asset Res
 */
using System;
using System.Collections;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.ResLoader
{
    public class ResourceAssetRes : Res, IPoolObject, IRunAsyncObject
    {
        public static ResourceAssetRes Allocate(string resName)
        {
            var res = ObjectPool<ResourceAssetRes>.Instance.Allocate();
            res.Init(resName);
            return res;
        }

        #region  IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ResourceAssetRes>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            ResPool.Recycle(assetName);
        }
        #endregion

        public void Init(string resName)
        {
            this.resName = resName;
            assetName = resName;
            resStatus = ResStatus.Waiting;
            resType = ResType.Resource;
            assetObject = null;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            resStatus = ResStatus.Loading;
            assetObject = Resources.Load(assetName);
            if (assetObject == null)
            {
                resStatus = ResStatus.Failed;
                return false;
            }
            resStatus = ResStatus.Ready;
            return true;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            if (resStatus == ResStatus.Ready)
            {
                BroadcastEvent(true);
            }
            else if (resStatus == ResStatus.Waiting || resStatus == ResStatus.Failed)
            {
                resStatus = ResStatus.Loading;
                RunAsync.Instance.Push(this);
            }
        }

        /// <summary>
        /// 执行异步加载
        /// </summary>
        /// <param name="async"></param>
        public IEnumerator AsyncRun(IRunAsync async)
        {
            var request = Resources.LoadAsync(assetName);

            yield return request;

            if (!request.isDone)
            {
                resStatus = ResStatus.Failed;
                async.OnRunAsync();
                BroadcastEvent(false);
                yield break;
            }

            assetObject = request.asset;

            if (assetObject == null)
            {
                resStatus = ResStatus.Failed;
                async.OnRunAsync();
                BroadcastEvent(false);
                yield break;
            }

            resStatus = ResStatus.Ready;
            async.OnRunAsync();
            BroadcastEvent(true);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload(bool unloadAllLoadedObjects = false)
        {
            Release();
        }

        /// <summary>
        /// 引用次数为0处理
        /// </summary>
        protected override void OnEmptyRef()
        {
            if (assetObject != null)
            {
                if (assetObject is GameObject) { }
                else Resources.UnloadAsset(assetObject);
            }
            assetObject = null;

            Recycle();
        }
    }
}
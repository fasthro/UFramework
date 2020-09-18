/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Asset
 */
using System;
using System.Collections;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class ResourceAsset : AssetObject, IPoolObject, IUCoroutineTaskRunner
    {
        public static ResourceAsset Allocate(string asset)
        {
            return ObjectPool<ResourceAsset>.Instance.Allocate().Build(asset); ;
        }

        #region  IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ResourceAsset>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            UAsset.Instance.RecycleAssetObject(assetName);
        }
        #endregion

        /// <summary>
        /// 构建 Resource Asset
        /// </summary>
        /// <param name="_asset"></param>
        private ResourceAsset Build(string _asset)
        {
            assetName = _asset;
            assetStatus = AssetStatus.Init;
            assetType = AssetType.Resource;
            assetObject = null;
            return this;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            assetStatus = AssetStatus.Loading;
            assetObject = Resources.Load(assetName);
            if (assetObject == null)
            {
                assetStatus = AssetStatus.Failed;
                return false;
            }
            assetStatus = AssetStatus.Loaded;
            return true;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public override void LoadAsync()
        {
            if (assetStatus == AssetStatus.Loaded)
            {
                BroadcastEvent(true);
            }
            else if (assetStatus == AssetStatus.Init || assetStatus == AssetStatus.Failed)
            {
                assetStatus = AssetStatus.Loading;
                UCoroutineTask.AddTaskRunner(this);
            }
        }

        /// <summary>
        /// 执行异步加载
        /// </summary>
        /// <param name="async"></param>
        public IEnumerator OnCoroutineTaskRun()
        {
            var request = Resources.LoadAsync(assetName);

            yield return request;

            if (!request.isDone)
            {
                assetStatus = AssetStatus.Failed;
                UCoroutineTask.TaskComplete();
                BroadcastEvent(false);
                yield break;
            }

            assetObject = request.asset;

            if (assetObject == null)
            {
                assetStatus = AssetStatus.Failed;
                UCoroutineTask.TaskComplete();
                BroadcastEvent(false);
                yield break;
            }

            assetStatus = AssetStatus.Loaded;
            UCoroutineTask.TaskComplete();
            BroadcastEvent(true);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload()
        {
            Release();
            if (isEmptyRef)
            {
                if (assetObject != null)
                {
                    Resources.UnloadAsset(assetObject);
                }
                assetObject = null;

                Recycle();
            }
        }
    }
}
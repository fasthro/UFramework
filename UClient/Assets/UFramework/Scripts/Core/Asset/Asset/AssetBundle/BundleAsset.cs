/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Asset
 */
using System.Collections;
using UFramework.Config;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class BundleAsset : AssetObject, IPoolObject, IUCoroutineTaskRunner
    {
        private AssetBundleRequest m_request;
        private Bundle m_bundle;

        /// <summary>
        /// 分配 AssetRes 对象
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static BundleAsset Allocate(string bundleName, string assetName)
        {
            return ObjectPool<BundleAsset>.Instance.Allocate().Build(bundleName, assetName);
        }

        #region IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<BundleAsset>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            UAsset.Instance.RecycleAssetObject(assetName);
            UAsset.Instance.RecycleAssetObject(bundleName);
        }
        #endregion

        /// <summary>
        /// 构建 Bundle Asset
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        private BundleAsset Build(string bundleName, string assetName)
        {
            this.bundleName = bundleName;
            this.assetName = assetName;
            assetStatus = AssetStatus.Init;
            assetType = AssetType.BundleAsset;
            assetObject = null;
            assetBundle = null;
            return this;
        }

        /// <summary>
        /// 搜索bundle
        /// </summary>
        private bool SearchAssetBundle()
        {
            if (assetBundle == null)
            {
                m_bundle = UAsset.Instance.GetAsset<Bundle>(bundleName, null, AssetType.Bundle);
                if (m_bundle != null)
                {
                    assetBundle = m_bundle.assetBundle;
                }
            }
            return assetBundle != null;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            if (assetStatus == AssetStatus.Loaded)
                return true;

            if (!SearchAssetBundle())
            {
                assetStatus = AssetStatus.Failed;
                return false;
            }
            assetStatus = AssetStatus.Loading;
            assetObject = assetBundle.LoadAsset(assetName);
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
                if (!SearchAssetBundle())
                {
                    assetStatus = AssetStatus.Failed;
                    BroadcastEvent(false);
                }
                else
                {
                    assetStatus = AssetStatus.Loading;
                    UCoroutineTask.AddTaskRunner(this);
                }
            }
        }

        /// <summary>
        /// 执行协同任务
        /// </summary>
        /// <returns></returns>
        public IEnumerator OnCoroutineTaskRun()
        {
            var request = assetBundle.LoadAssetAsync(assetName);

            m_request = request;
            yield return request;
            m_request = null;

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
                if (m_bundle != null)
                {
                    m_bundle.Unload();
                    m_bundle = null;
                }
                assetBundle = null;
                assetObject = null;

                Recycle();
            }
        }
    }
}
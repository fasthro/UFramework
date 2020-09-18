/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Bundle
 */
using System.Collections;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.Asset
{
    public class Bundle : AssetObject, IPoolObject, IUCoroutineTaskRunner
    {
        private AssetBundleCreateRequest m_request;
        private Bundle[] m_dependencies;
        private int m_dependWaitCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public static Bundle Allocate(string bundleName)
        {
            return ObjectPool<Bundle>.Instance.Allocate().Build(bundleName);
        }

        #region  IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<Bundle>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            UAsset.Instance.RecycleAssetObject(bundleName);
        }
        #endregion

        /// <summary>
        /// 构建 Bundle
        /// </summary>
        /// <param name="bundleName"></param>
        private Bundle Build(string bundleName)
        {
            this.bundleName = bundleName;
            assetStatus = AssetStatus.Init;
            assetType = AssetType.Bundle;
            assetObject = null;
            assetBundle = null;
            return this;
        }

        /// <summary>
        /// 查询依赖
        /// </summary>
        /// <returns></returns>
        private bool FindDependencies()
        {
            string[] dependencies = UAsset.Instance.GetDependencies(bundleName);
            if (dependencies != null)
            {
                int length = dependencies.Length;
                m_dependencies = new Bundle[length];
                for (int i = 0; i < length; i++)
                {
                    m_dependencies[i] = UAsset.Instance.GetAsset<Bundle>(dependencies[i], null, AssetType.Bundle);
                }
                return length > 0;
            }
            return false;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            if (assetStatus == AssetStatus.Loaded)
                return true;

            assetStatus = AssetStatus.Loading;

            // 先加载依赖
            if (FindDependencies())
            {
                bool dpass = true;
                for (int i = 0; i < m_dependencies.Length; i++)
                {
                    if (!m_dependencies[i].LoadSync())
                    {
                        dpass = false;
                    }
                }
                if (!dpass)
                {
                    assetStatus = AssetStatus.Failed;
                    return false;
                }
            }

            // 加载本体
            var url = IOPath.PathCombine(App.BundleDirectory, bundleName);
            assetBundle = AssetBundle.LoadFromFile(url);
            if (assetBundle == null)
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

                // 先加载依赖
                if (FindDependencies())
                {
                    m_dependWaitCount = m_dependencies.Length;
                    for (int i = 0; i < m_dependWaitCount; i++)
                    {
                        m_dependencies[i].AddListener(OnReceiveNotification);
                        m_dependencies[i].LoadAsync();
                    }
                }
                else
                {
                    UCoroutineTask.AddTaskRunner(this);
                }
            }
        }

        /// <summary>
        /// 接收通知处理依赖
        /// </summary>
        /// <param name="readly"></param>
        /// <param name="obj"></param>
        private void OnReceiveNotification(bool ready, AssetObject obj)
        {
            m_dependWaitCount--;
            obj.RemoveListener(OnReceiveNotification);
            if (m_dependWaitCount <= 0)
            {
                // 依赖加载完毕
                UCoroutineTask.AddTaskRunner(this);
            }
        }

        /// <summary>
        /// 执行异步加载
        /// </summary>
        /// <param name="async"></param>
        public IEnumerator OnCoroutineTaskRun()
        {
            var url = IOPath.PathCombine(App.BundleDirectory, bundleName); ;
            var request = AssetBundle.LoadFromFileAsync(url);

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

            assetBundle = request.assetBundle;

            if (assetBundle == null)
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
            // 依赖
            if (FindDependencies())
            {
                for (int i = 0; i < m_dependencies.Length; i++)
                {
                    m_dependencies[i].Unload();
                }
            }
            // 本体
            Release();
            if (isEmptyRef)
            {
                if (assetBundle != null)
                    assetBundle.Unload(true);
                assetBundle = null;
                assetObject = null;
                m_request = null;

                Recycle();
            }
        }
    }
}
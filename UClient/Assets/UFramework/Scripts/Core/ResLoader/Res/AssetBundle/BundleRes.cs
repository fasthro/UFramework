/*
 * @Author: fasthro
 * @Date: 2019-06-22 16:19:54
 * @Description: Assetbundle Res
 */
using System.Collections;
using UFramework.Coroutine;
using UFramework.Pool;
using UnityEngine;

namespace UFramework.ResLoader
{
    public class BundleRes : Res, IPoolObject, IUCoroutineTaskRunner
    {
        private AssetBundleCreateRequest m_request;
        private BundleRes[] m_dependencies;
        private int m_dependWaitCount;

        /// <summary>
        /// 分配 BundleRes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BundleRes Allocate(string resName)
        {
            var res = ObjectPool<BundleRes>.Instance.Allocate();
            res.Initialize(resName);
            return res;
        }

        #region  IPoolObject
        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<BundleRes>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            ResPool.Recycle(assetBundleName);
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="data"></param>
        private void Initialize(string resName)
        {
            assetBundleName = resName;
            resStatus = ResStatus.Waiting;
            resType = ResType.AssetBundle;
            assetObject = null;
            assetBundle = null;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <returns></returns>
        private bool FindDependencies()
        {
            string[] ds = GetDependencies(assetBundleName);
            int length = ds.Length;
            m_dependencies = new BundleRes[length];
            for (int i = 0; i < length; i++)
            {
                m_dependencies[i] = ResPool.Allocate<BundleRes>(ds[i], ResType.AssetBundle);
            }
            return length > 0;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public override bool LoadSync()
        {
            if (resStatus == ResStatus.Ready)
                return true;

            resStatus = ResStatus.Loading;

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
                    resStatus = ResStatus.Failed;
                    return false;
                }
            }

            // 加载本体
            var url = IOPath.PathCombine(App.BundleDirectory, assetBundleName);
            assetBundle = AssetBundle.LoadFromFile(url);
            if (assetBundle == null)
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
        /// <param name="res"></param>
        private void OnReceiveNotification(bool ready, Res res)
        {
            m_dependWaitCount--;
            res.RemoveListener(OnReceiveNotification);
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
            var url = IOPath.PathCombine(App.BundleDirectory, assetBundleName); ;
            var request = AssetBundle.LoadFromFileAsync(url);

            m_request = request;
            yield return request;
            m_request = null;

            if (!request.isDone)
            {
                resStatus = ResStatus.Failed;
                UCoroutineTask.TaskComplete();
                BroadcastEvent(false);
                yield break;
            }

            assetBundle = request.assetBundle;

            if (assetBundle == null)
            {
                resStatus = ResStatus.Failed;
                UCoroutineTask.TaskComplete();
                BroadcastEvent(false);
                yield break;
            }

            resStatus = ResStatus.Ready;
            UCoroutineTask.TaskComplete();
            BroadcastEvent(true);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public override void Unload(bool unloadAllLoadedObjects = true)
        {
            // 依赖
            if (FindDependencies())
            {
                for (int i = 0; i < m_dependencies.Length; i++)
                {
                    m_dependencies[i].Unload(unloadAllLoadedObjects);
                }
            }
            // 本体
            Release();
            if (isEmptyRef)
            {
                if (assetBundle != null)
                {
                    assetBundle.Unload(unloadAllLoadedObjects);
                }
                assetBundle = null;
                assetObject = null;
                m_request = null;

                Recycle();
            }
        }

        #region assebundle dependencies
        static AssetBundleManifest manifest;

        static string[] GetDependencies(string bundleName)
        {
            if (manifest == null)
            {
                // manifest
                var bundle = AssetBundle.LoadFromFile(IOPath.PathCombine(App.BundleDirectory, App.BundlePlatformName));
                manifest = bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                bundle.Unload(false);
            }
            return manifest.GetAllDependencies(bundleName);
        }
        #endregion
    }
}
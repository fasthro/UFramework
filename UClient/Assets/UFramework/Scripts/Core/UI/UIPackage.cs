/*
 * @Author: fasthro
 * @Date: 2020-09-29 13:22:48
 * @Description: package
 */
using System.Collections.Generic;
using UFramework.Config;
using UFramework.Messenger;
using UFramework.Ref;
using UnityEngine;

namespace UFramework.UI
{
    /// <summary>
    /// 加载状态
    /// </summary>
    public enum LoadState
    {
        Init,
        Loading,
        Loaded,
        Unloaded,
    }

    /// <summary>
    /// 包
    /// </summary>
    public abstract class Package : ReferenceObject
    {
        /// <summary>
        /// 包名称
        /// </summary>
        /// <value></value>
        public string packageName { get; private set; }

        /// <summary>
        /// 包加载状态
        /// </summary>
        public LoadState loadState { get; protected set; }

        protected HashSet<string> dependences = new HashSet<string>();
        protected int dependenCount;
        protected event UCallback onCompleted;
        protected bool isStandby;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageName"></param>
        public Package(string packageName)
        {
            this.packageName = packageName;
            this.loadState = LoadState.Init;
            this.isStandby = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onCompleted"></param>
        public void Load(UCallback onCompleted)
        {
            if (loadState == LoadState.Unloaded)
                return;

            Retain();
            isStandby = false;
            this.onCompleted += onCompleted;

            if (loadState == LoadState.Loaded) LoadCompleted();
            else if (loadState != LoadState.Loading && loadState != LoadState.Loaded)
            {
                loadState = LoadState.Loading;
                LoadMain();
            }
        }

        /// <summary>
        /// 加载主包
        /// </summary>
        protected virtual void LoadMain() { }

        /// <summary>
        /// 加载依赖包
        /// </summary>
        protected virtual void LoadDependen() { }


        /// <summary>
        /// 加载完成
        /// </summary>
        protected void LoadCompleted()
        {
            if (isStandby)
            {
                OnReferenceEmpty();
                return;
            }
            loadState = LoadState.Loaded;
            onCompleted.InvokeGracefully();
            onCompleted = null;
        }

        /// <summary>
        /// 卸载包引用
        /// </summary>
        public virtual void Unload()
        {
            Release();
        }

        /// <summary>
        /// 卸载释放包
        /// </summary>
        protected override void OnReferenceEmpty()
        {
            loadState = LoadState.Unloaded;
            onCompleted = null;
        }

        /// <summary>
        /// 待命-加载完成卸载资源
        /// </summary>
        public void Standby()
        {
            isStandby = true;
        }
    }

    /// <summary>
    /// 包代理者
    /// </summary>
    public static class PackageAgents
    {
        /// <summary>
        /// 包映射
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="Package"></typeparam>
        /// <returns></returns>
        readonly static Dictionary<string, Package> packageDictonary = new Dictionary<string, Package>();

        /// <summary>
        /// 未加载完成，直接被卸载包映射
        /// 等待包加载完毕之后进行卸载，在此期间会继续执行加载，并设置标记
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <typeparam name="Package"></typeparam>
        /// <returns></returns>
        readonly static Dictionary<string, Package> standbyDictonary = new Dictionary<string, Package>();

        /// <summary>
        /// 加载 Fairy Resource 包
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="onCompleted"></param>
        public static void LoadFairyResource(string packageName, UCallback onCompleted)
        {
            _Load(packageName, onCompleted, true);
        }

        /// <summary>
        /// 加载包
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="onCompleted"></param>
        public static void Load(string packageName, UCallback onCompleted)
        {
            _Load(packageName, onCompleted, false);
        }

        /// <summary>
        /// 加载包
        /// 在Standby中查找包是否正在加载的包，是则移除Standby标记继续执行加载
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="onCompleted"></param>
        /// <param name="isResourcesPackage"></param>
        private static void _Load(string packageName, UCallback onCompleted, bool isResourcesPackage)
        {
            Package package;
            if (standbyDictonary.TryGetValue(packageName, out package))
            {
                if (package.loadState == LoadState.Loading)
                {
                    standbyDictonary.Remove(packageName);
                    packageDictonary.Add(packageName, package);
                }
                else standbyDictonary.Remove(packageName);
            }

            if (package == null)
            {
                if (!packageDictonary.TryGetValue(packageName, out package))
                {
                    if (UConfig.Read<AppConfig>().useFairyGUI)
                    {
                        if (isResourcesPackage) package = new FairyResourcesPackage(packageName);
                        else package = new FairyPackage(packageName);
                    }

                    packageDictonary.Add(packageName, package);
                }
            }
            package.Load(onCompleted);
        }

        /// <summary>
        /// 卸载包
        /// </summary>
        /// <param name="packageName"></param>
        public static void Unload(string packageName)
        {
            Package package;
            if (packageDictonary.TryGetValue(packageName, out package))
            {
                if (package.loadState == LoadState.Loading)
                {
                    package.Standby();
                    standbyDictonary.Add(packageName, package);
                    packageDictonary.Remove(packageName);
                }
                else
                {
                    package.Unload();
                    if (package.isEmptyRef)
                    {
                        packageDictonary.Remove(packageName);
                    }
                }
            }
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-09-29 13:22:48
 * @Description: package
 */
using System.Collections.Generic;
using UFramework.Config;
using UFramework.Messenger;
using UFramework.Ref;
using UFramework.UI.FairyGUI;

namespace UFramework.UI
{
    /// <summary>
    /// 加载状态
    /// </summary>
    public enum LoadState
    {
        Init,
        Load,
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

        protected event UCallback m_onCompleted;
        protected bool m_remain;

        public Package(string packageName)
        {
            this.packageName = packageName;
            this.loadState = LoadState.Init;
            this.m_remain = false;
        }

        /// <summary>
        /// 加载包
        /// </summary>
        /// <param name="onCompleted"></param>
        public virtual void Load(UCallback onCompleted)
        {
            Retain();
            loadState = LoadState.Load;
            m_remain = false;
            m_onCompleted += onCompleted;
        }

        /// <summary>
        /// 资源加载完成
        /// </summary>
        protected void OnLoadAsset()
        {
            if (m_remain)
            {
                OnReferenceEmpty();
                return;
            }

            loadState = LoadState.Loaded;
            m_onCompleted.InvokeGracefully();
            m_onCompleted = null;
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
            m_onCompleted = null;
        }

        /// <summary>
        /// 标记稍后卸载
        /// </summary>
        public void Remain()
        {
            m_remain = true;
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
        readonly static Dictionary<string, Package> remainDictonary = new Dictionary<string, Package>();

        /// <summary>
        /// 加载包
        /// 在remain中查找包是否正在加载的包，是则移除remain标记继续执行加载
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="onCompleted"></param>
        public static void Load(string packageName, UCallback onCompleted)
        {
            Package package;
            if (remainDictonary.TryGetValue(packageName, out package))
            {
                if (package.loadState == LoadState.Load)
                {
                    remainDictonary.Remove(packageName);
                    packageDictonary.Add(packageName, package);
                }
                else remainDictonary.Remove(packageName);
            }

            if (package == null)
            {
                if (!packageDictonary.TryGetValue(packageName, out package))
                {
                    if (UConfig.Read<AppConfig>().useFairyGUI)
                    {
                        package = new FairyPackage(packageName);
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
                if (package.loadState == LoadState.Load)
                {
                    package.Remain();
                    remainDictonary.Add(packageName, package);
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
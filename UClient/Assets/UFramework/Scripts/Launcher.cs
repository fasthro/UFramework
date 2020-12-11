/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using FairyGUI;
using UFramework.Core;
using UFramework.NativePlatform;
using UFramework.UI;
using UnityEngine;

namespace UFramework
{
    [MonoSingletonPath("UFramework")]
    public class Launcher : MonoSingleton<Launcher>
    {
        public static Launcher Main { get { return Launcher.Instance; } }
        public static GameObject MainGameObject { get { return Launcher.Instance.gameObject; } }
        public static bool Develop { get; private set; }

        private Launch _launch;
        private bool _initialized;

        protected override void OnSingletonAwake()
        {
            _initialized = false;
            var serdata = Core.Serializer<AppConfig>.Instance;

            // FairyGUI
            UIPackage.unloadBundleByFGUI = false;
            GRoot.inst.SetContentScaleFactor(serdata.designResolutionX, serdata.designResolutionY, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);

            // launch panel
            _launch = Launch.Create();

            // 日志等级
            Logger.SetLevel(serdata.logLevel);
            // 线程队列
            ThreadQueue.Instance.Default();
            // Native
            Native.Instance.Default();
            // 定时器
            Timer.Instance.Default();
            // 下载器
            Downloader.Instance.Default();
            // 版本器
            Updater.Instance.StartUpdate(_launch, OnUpdaterCompleted);
        }

        private void OnUpdaterCompleted()
        {
            // 资源
            Assets.Instance.Initialize(OnAssetCompleted);
        }

        private void OnAssetCompleted(bool result)
        {
            _initialized = true;

            // 启动服务
            Service.Instance.Default();

            // 启动luaEngine
            Service.Instance.container.GetService<ManagerService>().container.GetManager<LuaManager>().LaunchEngine();

            _launch.ShowTouchBeginOperation(OnRunner);
        }

        private void OnRunner()
        {
            _launch.Dispose();
            _launch = null;
        }
    }
}
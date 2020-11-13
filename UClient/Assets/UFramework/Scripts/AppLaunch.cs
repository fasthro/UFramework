/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using FairyGUI;
using UFramework.Assets;
using UFramework.Config;
using UFramework.Natives;
using UFramework.Panel.FairyGUI;
using UFramework.Timers;
using UFramework.Tools;
using UFramework.VersionControl;
using UnityEngine;

namespace UFramework
{
    [MonoSingletonPath("UFramework")]
    public class AppLaunch : MonoSingleton<AppLaunch>
    {
        public static AppLaunch main { get { return AppLaunch.Instance; } }
        public static GameObject mainGameObject { get { return AppLaunch.Instance.gameObject; } }

        public static Launch launch { get; private set; }

        private bool _initialized = false;

        protected override void OnSingletonAwake()
        {
            _initialized = false;

            var config = UConfig.Read<AppConfig>();
            // FairyGUI
            UIPackage.unloadBundleByFGUI = false;
            GRoot.inst.SetContentScaleFactor(config.designResolutionX, config.designResolutionY, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);

            // launch panel
            launch = Launch.Create();

            // 线程队列
            ThreadQueue.Instance.Default();
            // Native
            Native.Instance.Default();
            // 定时器
            Timer.Instance.Default();
            // 下载器
            Download.Instance.Default();
            // 版本器
            Updater.Instance.StartUpdate(launch, OnUpdaterCompleted);
        }

        private void OnUpdaterCompleted()
        {
            // 资源
            Asset.Instance.Initialize(OnAssetCompleted);
        }

        private void OnAssetCompleted(bool result)
        {
            _initialized = true;
            App.Initialize();
            launch.ShowTouchBeginOperation(OnStartNewWorld);
        }

        private void OnStartNewWorld()
        {
            launch.Dispose();
            launch = null;

            App.GetManager<LuaManager>().luaEngine.Call("runner");
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (_initialized)
                App.Update(deltaTime);
        }

        protected override void OnSingletonLateUpdate()
        {
            if (_initialized)
                App.LateUpdate();
        }

        protected override void OnSingletonFixedUpdate()
        {
            if (_initialized)
                App.FixedUpdate();
        }

        protected override void OnSingletonDestory()
        {
            if (_initialized)
                App.Destory();
        }
    }
}
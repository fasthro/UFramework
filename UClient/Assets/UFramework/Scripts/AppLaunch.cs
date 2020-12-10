/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using FairyGUI;
using UFramework.Assets;
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
        public static AppLaunch Main { get { return AppLaunch.Instance; } }
        public static GameObject MainGameObject { get { return AppLaunch.Instance.gameObject; } }
        public static bool Develop { get; private set; }

        private Launch _launch;
        private bool _initialized;

        protected override void OnSingletonAwake()
        {
            _initialized = false;
            var serdata = Serialize.Serializable<AppSerdata>.Instance;

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
            Download.Instance.Default();
            // 版本器
            Updater.Instance.StartUpdate(_launch, OnUpdaterCompleted);
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
            _launch.ShowTouchBeginOperation(OnRunner);
        }

        private void OnRunner()
        {
            _launch.Dispose();
            _launch = null;

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
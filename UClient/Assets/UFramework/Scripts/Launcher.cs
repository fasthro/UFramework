/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */
using System.Collections;
using FairyGUI;
using UFramework.Core;
using UFramework.NativePlatform;
using UFramework.UI;
using UnityEngine;
using UnityEngine.Events;

namespace UFramework
{
    [MonoSingletonPath("UFramework")]
    public class Launcher : MonoSingleton<Launcher>
    {
        public static Launcher Main { get { return Launcher.Instance; } }
        public static GameObject MainGameObject { get { return Launcher.Instance.gameObject; } }
        public static bool Develop { get; private set; }

        public UnityEvent launchedListener;

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
            // 启动服务
            Service.Instance.Default();
            // 版本器
            Updater.Instance.StartUpdate(_launch, () =>
            {
                // 资源
                Assets.Instance.Initialize((succeed) =>
                {
                    if (succeed)
                    {
                        Launched();
                    }
                    else Core.Coroutine.Allocate(OnAssetFailed());
                });
            });
        }

        IEnumerator OnAssetFailed()
        {
            var mb = MessageBox.Allocate().Show("提示", "资源初始化失败,请检查资源", "退出").OnlyOK();
            yield return mb;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Launched()
        {
            _initialized = true;

            // 启动luaEngine
            Service.Instance.GetService<ManagerService>().GetManager<LuaManager>().LaunchEngine();

            launchedListener?.Invoke();

            _launch.Hide();
            _launch.Dispose();
            _launch = null;
        }
    }
}
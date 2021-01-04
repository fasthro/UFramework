/*
 * @Author: fasthro
 * @Date: 2020-07-13 23:51:59
 * @Description: App Launch
 */

using FairyGUI;
using System.Collections;
using UFramework.Core;
using UFramework.NativePlatform;
using UFramework.UI;
using UnityEngine;

namespace UFramework
{
    public abstract class AppLauncher : MonoBehaviour
    {
        public static AppLauncher Main { get; private set; }
        public static GameObject MainGameObject { get { return Main.gameObject; } }
        public static bool Develop { get; private set; }

        public void Initialize()
        {
            Main = this;
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
            // Init
            InitBehaviour();
            // 版本器
            Updater.Instance.StartUpdate(_launch, () =>
            {
                // 资源
                Assets.Instance.Initialize((succeed) =>
                {
                    if (succeed)
                    {
                        _initialized = true;
                        _managerContainer.GetManager<LuaManager>().LaunchEngine(_managerContainer);

                        OnInitialized();

                        _launch.Hide();
                        _launch.Dispose();
                        _launch = null;
                    }
                    else Core.Coroutine.Allocate(OnAssetFailed());
                });
            });
        }

        protected ManagerContainer _managerContainer;

        protected BaseManager[] _allManagers;

        protected void DoUpdate(float deltaTime)
        {
            foreach (var manager in _allManagers)
            {
                manager.Update(deltaTime);
            }
        }

        protected void DoDispose()
        {
            foreach (var manager in _allManagers)
            {
                manager.Dispose();
            }
        }

        protected void DoLateUpdate()
        {
            foreach (var manager in _allManagers)
            {
                manager.LateUpdate();
            }
        }

        protected void FixedUpdate()
        {
            foreach (var manager in _allManagers)
            {
                manager.FixedUpdate();
            }
        }

        protected void DoApplicationQuit()
        {
            foreach (var manager in _allManagers)
            {
                manager.ApplicationQuit();
            }
        }

        protected virtual void InitManager()
        {
            _managerContainer = new ManagerContainer();
            BaseBehaviour.SetContainer(_managerContainer);

            _managerContainer.RegisterManager(new LuaManager());
            _managerContainer.RegisterManager(new NetworkManager());
            _managerContainer.RegisterManager(new ResManager());
        }

        protected virtual void OnInitialized()
        {
        }

        private Launch _launch;

        private bool _initialized;

        private void InitBehaviour()
        {
            InitManager();

            _allManagers = _managerContainer.GetAllManagers();
            foreach (var manager in _allManagers)
            {
                (manager as BaseBehaviour).SetReference();
            }
        }

        private IEnumerator OnAssetFailed()
        {
            var mb = MessageBox.Allocate().Show("提示", "资源初始化失败,请检查资源", "退出").OnlyOK();
            yield return mb;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
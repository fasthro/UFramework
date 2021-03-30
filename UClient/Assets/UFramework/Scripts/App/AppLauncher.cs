// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-07-13 23:51:59
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using System.Collections;
using UFramework.Core;
using UFramework.NativePlatform;
using UFramework.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UFramework
{
    public abstract class AppLauncher : MonoBehaviour
    {
        public static AppLauncher Main { get; private set; }
        public static GameObject MainGameObject => Main.gameObject;

        public static bool Develop { get; private set; }
        public ManagerContainer managerContainer { get; private set; }
        protected bool isInitialized { get; private set; }

        protected BaseManager[] _allManagers;

        private LaunchPanel _launchPanel;

        protected void Initialize()
        {
            Main = this;
            isInitialized = false;
            var appConfig = Serializer<AppConfig>.Instance;

            if (Platform.RuntimePlatformCurrent == Platform.StandaloneWindows
                || Platform.RuntimePlatformCurrent == Platform.StandaloneOSX)
            {
                if (appConfig.fullScreenMode != FullScreenMode.FullScreenWindow)
                {
                    Screen.fullScreen = false;
                    Screen.SetResolution(appConfig.resolutionX, appConfig.resolutionY, appConfig.fullScreenMode);
                }
                else
                {
                    Screen.fullScreen = true;
                }
            }
            else
            {
                Screen.fullScreen = true;
            }

            #region FairyGUI

            // font
            foreach (var fontName in appConfig.fonts)
            {
                var font = Resources.Load<Font>($"Font/{fontName}");
                if (font != null)
                    FontManager.RegisterFont(new DynamicFont(fontName, font));
                else Debug.LogError($"Font {fontName} Registration failed.");
            }

            UIPackage.unloadBundleByFGUI = false;
            GRoot.inst.SetContentScaleFactor(appConfig.designResolutionX, appConfig.designResolutionY, UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);

            // URP项目下摄像机堆叠处理
            if (appConfig.isURP)
            {
                var stageCamera = StageCamera.main;
                stageCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
                Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(stageCamera);
            }

            #endregion

            // launch panel
            _launchPanel = LaunchPanel.Create();
            _launchPanel.Show();

            // Console
            Console.Instance.Default();
            // 日志等级
            Logger.SetLevel(appConfig.logLevel);
            // 线程队列
            ThreadQueue.Instance.Default();
            // Native
            Native.Instance.Default();
            // 定时器
            Timer.Instance.Default();
            // 下载器
            Downloader.Instance.Default();
            // GoPool
            GoPool.Instance.Default();
            
            Logger.Info("UFramework Base Initialized");
            
            // 版本器
            Updater.Instance.StartUpdate(_launchPanel, () =>
            {
                Logger.Info("UFramework Updater Initialized");
                
                // 资源
                Assets.Instance.Initialize((succeed) =>
                {
                    if (succeed)
                    {
                        Logger.Info("UFramework Initialized.");
                        
                        isInitialized = true;
                        // Init
                        InitBehaviour();
                        managerContainer.GetManager<LuaManager>().LaunchEngine(managerContainer);
                        OnInitialized();

                        _launchPanel.Hide();
                        _launchPanel.window.Dispose();
                        _launchPanel = null;
                    }
                    else Core.Coroutine.Allocate(OnAssetFailed());
                });
            });
        }

        protected void DoUpdate(float deltaTime)
        {
            if (!isInitialized)
                return;

            foreach (var manager in _allManagers)
            {
                manager.Update(deltaTime);
            }
        }

        protected void DoDispose()
        {
            if (!isInitialized)
                return;

            foreach (var manager in _allManagers)
            {
                manager.Dispose();
            }
        }

        protected void DoLateUpdate()
        {
            if (!isInitialized)
                return;

            foreach (var manager in _allManagers)
            {
                manager.LateUpdate();
            }
        }

        protected void DoFixedUpdate()
        {
            if (!isInitialized)
                return;

            foreach (var manager in _allManagers)
            {
                manager.FixedUpdate();
            }
        }

        protected void DoApplicationQuit()
        {
            if (!isInitialized)
                return;

            foreach (var manager in _allManagers)
            {
                manager.ApplicationQuit();
            }
        }

        protected virtual void InitManager()
        {
            managerContainer = new ManagerContainer();
            BaseBehaviour.SetContainer(managerContainer);

            managerContainer.RegisterManager(new LuaManager());
            managerContainer.RegisterManager(new NetworkManager());
            managerContainer.RegisterManager(new ResManager());
            managerContainer.RegisterManager(new AdapterManager());
        }

        protected virtual void OnInitialized()
        {
        }

        private void InitBehaviour()
        {
            InitManager();

            _allManagers = managerContainer.GetAllManagers();
            foreach (var manager in _allManagers)
            {
                ((BaseBehaviour) manager).SetReference();
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
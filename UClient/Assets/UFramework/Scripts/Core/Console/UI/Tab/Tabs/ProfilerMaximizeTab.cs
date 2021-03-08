// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 14:52
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;

namespace UFramework.Consoles
{
    public class ProfilerMaximizeTab : BaseConsoleTab, IConsolePanelTab
    {
        private static FPSService fpsService;

        #region component

        private GTextField _fps;

        private GButton _minimizeBtn;
        private GButton _gcBtn;
        private GButton _cleanBtn;

        #endregion

        public ProfilerMaximizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            fpsService = Console.Instance.GetService<FPSService>();
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_profilerMaxTab").asCom;

            _fps = _view.GetChild("_fps").asTextField;
            
            _minimizeBtn = _view.GetChild("_minimize").asButton;
            _minimizeBtn.onClick.Set(OnMinimizeClick);
            
            var monoCom = _view.GetChild("_mono").asCom;
            monoCom.GetChild("_title").text = "Mono Memory Usage";
            
            _gcBtn = monoCom.GetChild("_btn").asButton;
            _gcBtn.onClick.Set(OnGCClick);
            
            var usageCom = _view.GetChild("_usage").asCom;
            usageCom.GetChild("_title").text = "Memory Usage";
            
            _cleanBtn = usageCom.GetChild("_btn").asButton;
            _cleanBtn.onClick.Set(OnCleanClick);
        }

        public void DoShow()
        {
            Initialize();
        }

        public void DoHide()
        {
        }

        public void DoRefresh()
        {
        }

        public void DoUpdate()
        {
            if (!initialized)
                return;

            _fps.text = $"FPS:{fpsService.GetFPS(2).ToString()}";
        }

        private void OnMinimizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.ProfilerMinimize);
        }
        
        private void OnGCClick()
        {
            
        }
        
        private void OnCleanClick()
        {
            
        }
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 14:52
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;

namespace UFramework.Consoles
{
    public class ProfilerMinimizeTab : BaseConsoleTab, IConsolePanelTab
    {
        private static FPSService fpsService;

        #region component

        private GTextField _fps;

        #endregion
        
        public ProfilerMinimizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            fpsService = Console.Instance.GetService<FPSService>();
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_profilerMinTab").asCom;

            _fps = _view.GetChild("_fps").asTextField;
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
    }
}
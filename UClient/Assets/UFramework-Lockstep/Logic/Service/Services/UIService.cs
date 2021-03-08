// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 15:06
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework;
using UFramework.Consoles;
using UFramework.Core;

namespace Lockstep.Logic
{
    public class UIService : BaseGameService, IUIService
    {
        private static AdapterManager adapterManager;
        private static FPSService fpsService;

        public GComponent view { get; private set; }

        #region ui

        private GTextField _pingText;
        private GTextField _delayText;
        private GTextField _fpsText;

        #endregion

        public override void Initialize()
        {
            adapterManager = global::Launcher.instance.managerContainer.GetManager<AdapterManager>();
            fpsService = Console.Instance.GetService<FPSService>();
            
            Messenger.AddListener(EventDefine.GAME_INIT, OnGameInit);
        }

        public override void Update()
        {
            if (_fpsText != null)
                _fpsText.text = fpsService.GetFPS(0);
        }

        private void OnGameInit()
        {
            view = adapterManager.FairyQueryComponent("MainPanel", "");

            _pingText = view.GetChild("_ping").asTextField;
            _delayText = view.GetChild("_delay").asTextField;
            _fpsText = view.GetChild("_fps").asTextField;
        }

        public void UpdatePing(int ping)
        {
            _pingText.text = $"{ping}ms";
        }

        public void UpdateDelay(int delay)
        {
            _delayText.text = $"{delay}ms";
        }
    }
}
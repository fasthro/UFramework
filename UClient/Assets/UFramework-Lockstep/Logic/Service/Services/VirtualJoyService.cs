// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 15:35
// * @Description:
// --------------------------------------------------------------------------------

using UFramework;
using UFramework.Core;
using UFramework.Game;

namespace Lockstep.Logic
{
    public class VirtualJoyService : BaseGameBehaviour, IVirtualJoyService
    {
        /// <summary>
        /// 移动摇杆
        /// </summary>
        public JoyMove move { get; private set; }

        public override void Initialize()
        {
            Messenger.AddListener(EventDefine.GAME_INIT, OnGameInit);
            Messenger.AddListener(EventDefine.GAME_START, OnGameStart);
        }

        private void OnGameInit()
        {
            move = new JoyMove(VirtualJoy.Instance.GetJoyWithName("Move Joy"));
        }

        private void OnGameStart()
        {
            var moveCom = _uiService.view.GetChild("_moveJoy").asCom;
            move.ui = moveCom;
            move.slider = moveCom.GetChild("_touch").asImage;
            move.startTransition = moveCom.GetTransition("_touchBegin");
            move.endTransition = moveCom.GetTransition("_touchEnd");
            move.Initialize();
        }
    }
}
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
    public class VirtualJoyService : BaseGameBehaviour, IVirtualJoyService, IGameRuntime
    {
        /// <summary>
        /// 移动摇杆
        /// </summary>
        public JoyMove move { get; private set; }
        
        public void InitGame(GameStartMessage message)
        {
            move = new JoyMove(VirtualJoy.Instance.GetJoyWithName("Move Joy"));
            
            var moveCom = _uiService.view.GetChild("_moveJoy").asCom;
            move.ui = moveCom;
            move.slider = moveCom.GetChild("_touch").asImage;
            move.startTransition = moveCom.GetTransition("_touchBegin");
            move.endTransition = moveCom.GetTransition("_touchEnd");
            move.Initialize();
        }
    }
}
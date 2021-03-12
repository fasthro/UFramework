// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:32:13
// * @Description:
// --------------------------------------------------------------------------------

using UFramework;
using UFramework.Core;
using UFramework.Game;
using UnityEngine;

namespace Lockstep.Logic
{
    public class InputService : BaseGameService, IInputService, IGameRuntime
    {
        public InputData inputData { get; private set; }

        public void InitGame(GameStartMessage message)
        {
            _virtualJoyService.move.moveListener += OnMoveVirtualJoy;
        }

        private void OnMoveVirtualJoy(Vector2 value, bool isMove)
        {
            if (!isMove || _playerService.self == null)
            {
                inputData = null;
            }
            else
            {
                inputData = ObjectPool<InputData>.Instance.Allocate();
                inputData.movementDir.x = (FP) value.x;
                inputData.movementDir.z = (FP) value.y;
            }
        }
    }
}
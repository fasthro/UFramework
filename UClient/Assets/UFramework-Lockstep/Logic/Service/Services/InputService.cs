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
    public class InputService : BaseGameService, IInputService
    {
        public InputData inputData { get; private set; }

        public override void Initialize()
        {
            Messenger.AddListener(EventDefine.GAME_START, OnGameStart);
        }

        private void OnGameStart()
        {
            _virtualJoyService.move.moveListener += OnMoveVirtualJoy;
        }

        private void OnMoveVirtualJoy(Vector2 value, bool isMove)
        {
            if (isMove && _playerService.self == null) return;

            inputData = ObjectPool<InputData>.Instance.Allocate();
            inputData.movementDir.x = (FP) value.x;
            inputData.movementDir.z = (FP) value.y;
        }
    }
}
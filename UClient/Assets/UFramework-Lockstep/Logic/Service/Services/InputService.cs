// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:32:13
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace Lockstep.Logic
{
    public class InputService : BaseGameService, IInputService
    {
        public InputData inputData => _inputData;

        private InputData _inputData;

        public override void Update()
        {
            if (_playerService.self == null)
                return;

            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            _inputData = ObjectPool<InputData>.Instance.Allocate();
            _inputData.movementDir.x = (FP) h;
            _inputData.movementDir.z = (FP) v;
        }

        public void ExecuteInputData(GameEntity entity, InputData input)
        {
            entity.ReplaceCMovement(input.movementDir, false);
        }
    }
}
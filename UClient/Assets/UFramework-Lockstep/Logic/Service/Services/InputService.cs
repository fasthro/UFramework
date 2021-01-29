/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:47:33
 * @Description: 
 */

using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Lockstep.MessageData;

namespace Lockstep.Logic
{
    public class InputService : BaseGameService, IInputService
    {
        public override void Update()
        {
            if (_agentService.self == null)
                return;

            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");

            var input = ObjectPool<AgentInput>.Instance.Allocate();
            input.inputDirection.x = (FP) h;
            input.inputDirection.z = (FP) v;
            _agentService.self.inputs.Clear();
            _agentService.self.inputs.Add(input);
        }

        public void ExecuteInput(GameEntity entity, AgentInput input)
        {
            entity.ReplaceCMovement(input.inputDirection);
        }
    }
}
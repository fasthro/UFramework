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
            if (_agentService.selfAgent == null)
                return;
            
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");
            
            var input = ObjectPool<AgentInput>.Instance.Allocate();
            input.position.X = h;
            input.position.Z = v;
            _agentService.selfAgent.CleanInputs();
            _agentService.selfAgent.AddInput(input);
        }

        public void ExecuteInput(Agent agent, AgentInput input)
        {
            agent.entity.ReplaceCPosition(input.position);
        }
    }
}
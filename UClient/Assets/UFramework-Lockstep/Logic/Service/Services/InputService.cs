/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:47:33
 * @Description: 
 */
using System.Collections.Generic;
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

            var input = ObjectPool<AgentInput>.Instance.Allocate();
            input.position = Input.mousePosition.ToCS();
            _agentService.selfAgent.AddInput(input);
        }
    }
}
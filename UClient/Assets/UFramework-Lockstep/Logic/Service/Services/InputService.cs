/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:47:33
 * @Description: 
 */
using UnityEngine;

namespace Lockstep.Logic
{
    public class InputService : BaseGameService, IInputService
    {
        public PlayerInput curentInput => _curentInput;
        private PlayerInput _curentInput;
        public override void Update()
        {
            _curentInput = new PlayerInput()
            {
                px = Input.mousePosition.x,
                py = Input.mousePosition.y
            };
        }
    }
}
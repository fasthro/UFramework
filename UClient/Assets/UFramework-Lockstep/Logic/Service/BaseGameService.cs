/*
 * @Author: fasthro
 * @Date: 2021/1/4 11:23:52
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockstep.Logic
{
    public class BaseGameService : BaseService
    {
        protected IInputService _inputService;

        public override void SetReference()
        {
            base.SetReference();
            _inputService = _container.GetService<IInputService>();
        }
    }
}
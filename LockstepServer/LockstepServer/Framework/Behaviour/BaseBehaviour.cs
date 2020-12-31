using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public abstract class BaseBehaviour : BaseService
    {
        public BaseBehaviour(ServiceContainer serviceContainer)
        {
            SetReference(serviceContainer);
        }
    }
}
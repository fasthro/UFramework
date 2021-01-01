using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    public abstract class BaseGameBehaviour : BaseGameService
    {
        public BaseGameBehaviour()
        {
            SetReference();
        }
    }
}
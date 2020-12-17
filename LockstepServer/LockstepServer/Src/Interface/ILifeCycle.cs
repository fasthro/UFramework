using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public interface ILifeCycle
    {
        void Initialize();
        void DoUpdate(float deltaTime);
        void DoDispose();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public abstract class BaseBehaviour : ILifeCycle
    {
        public BaseBehaviour()
        {
            Initialize();        
        }

        public void Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public void DoUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }

        public void DoDispose()
        {
            OnDispose();
        }

        protected void OnDispose() { }

    }
}

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

        public void DoUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public void DoDispose()
        {
            OnDispose();
        }

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        protected virtual void OnDispose()
        {
        }
    }
}
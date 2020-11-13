/*
 * @Author: fasthro
 * @Date: 2020-05-25 23:52:22
 * @Description: BaseManager
 */

using UnityEngine;

namespace UFramework
{
    public abstract class BaseManager
    {
        public void Initialize()
        {
            OnInitialize();
        }

        protected abstract void OnInitialize();

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected abstract void OnUpdate(float deltaTime);

        public void LateUpdate()
        {
            OnLateUpdate();
        }

        protected abstract void OnLateUpdate();

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

        protected abstract void OnFixedUpdate();

        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnDispose();

        protected virtual void Log(object message)
        {
            Debug.Log(message);
        }

        protected virtual void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-05-25 23:52:22
 * @Description: BaseManager
 */

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

        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnDispose();
    }
}
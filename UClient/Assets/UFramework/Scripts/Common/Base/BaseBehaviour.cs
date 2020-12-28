/*
 * @Author: fasthro
 * @Date: 2020-12-28 11:02:48
 * @Description: 
 */
namespace UFramework
{
   public abstract class BaseBehaviour : ILifeCycle
    {
        public void Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public void Update(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
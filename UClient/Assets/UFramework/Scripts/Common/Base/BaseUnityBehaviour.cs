/*
 * @Author: fasthro
 * @Date: 2020-12-28 11:02:48
 * @Description: 
 */
namespace UFramework
{
    public abstract class BaseUnityBehaviour : BaseBehaviour, IUnityLifeCycle
    {
        public void LateUpdate()
        {
            OnLateUpdate();
        }

        protected virtual void OnLateUpdate() { }

        public void FixedUpdate()
        {
            OnFixedUpdate();
        }

        protected virtual void OnFixedUpdate() { }

        public void ApplicationQuit()
        {
            OnApplicationQuit();
        }

        protected virtual void OnApplicationQuit() { }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-12-11 17:37:22
 * @Description: 生命周期接口
 */
namespace UFramework
{
    public interface ILifeCycle
    {
        void OnAwake();
        void OnStart();
        void OnUpdate(float deltaTime);
        void OnLateUpdate();
        void OnFixedUpdate();
        void OnDestroy();
        void OnApplicationQuit();
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:44:00
 * @Description:
 */

namespace LockstepServer
{
    public interface ILifeCycle
    {
        void DoDispose();

        void DoUpdate(float deltaTime);

        void Initialize();
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:44:00
 * @Description:
 */

namespace LockstepServer
{
    public interface IBehaviour
    {
        void Initialize();

        void Update();

        void Dispose();
    }
}
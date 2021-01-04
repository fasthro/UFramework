/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:42:46
 * @Description:
 */

namespace UFramework
{
    public interface IBehaviour
    {
        void Initialize();

        void Update(float deltaTime);

        void Dispose();

        void LateUpdate();

        void FixedUpdate();

        void ApplicationQuit();
    }
}
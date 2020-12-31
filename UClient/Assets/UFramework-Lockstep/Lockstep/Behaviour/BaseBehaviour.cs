/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:38:01
 * @Description:
 */

namespace Lockstep
{
    public abstract class BaseBehaviour : BaseService
    {
        public BaseBehaviour(ServiceContainer serviceContainer)
        {
            SetReference(serviceContainer);
        }
    }
}
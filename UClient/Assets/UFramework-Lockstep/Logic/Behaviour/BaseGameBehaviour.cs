/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:38:01
 * @Description:
 */

namespace Lockstep.Logic
{
    public abstract class BaseGameBehaviour : BaseGameService
    {
        public BaseGameBehaviour() : base()
        {
            SetReference();
        }
    }
}
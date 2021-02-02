// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 17:37
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public abstract class BaseAgent
    {
        public GameEntity entity { get; protected set; }

        public IView view => entity.cView.value;
    }
}
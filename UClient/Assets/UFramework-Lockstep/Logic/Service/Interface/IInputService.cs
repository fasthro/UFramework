// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:32:13
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep.Logic
{
    public interface IInputService : IService
    {
        InputData inputData { get; }
        void ExecuteInputData(GameEntity entity, InputData input);
    }
}
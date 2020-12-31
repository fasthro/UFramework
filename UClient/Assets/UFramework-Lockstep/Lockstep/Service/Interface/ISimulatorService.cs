/*
 * @Author: fasthro
 * @Date: 2020/12/30 19:38:57
 * @Description:
 */

namespace Lockstep
{
    public interface ISimulatorService : IService
    {
        void Start(GameStartData data);
    }
}
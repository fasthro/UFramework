// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 15:35
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Game;

namespace Lockstep.Logic
{
    public interface IVirtualJoyService : IService
    {
        JoyMove move { get; }
    }
}
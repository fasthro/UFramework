// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 15:07
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;

namespace Lockstep.Logic
{
    public interface IUIService: IService
    {
        GComponent view { get; }

        void UpdatePing(int ping);
        void UpdateDelay(int delay);
    }
}
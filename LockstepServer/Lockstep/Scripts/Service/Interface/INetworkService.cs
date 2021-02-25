// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public interface INetworkService : IService
    {
        void SendPing();
        void SendInput(FrameData frameData);
    }
}
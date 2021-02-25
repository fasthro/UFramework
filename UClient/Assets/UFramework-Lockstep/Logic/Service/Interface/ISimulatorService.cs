// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep.Logic
{
    public interface ISimulatorService : IService
    {
        void OnReceiveGameStart(GameStartMessage message);
        void OnReceiveFrame(FrameData frame);
        void OnReceivePing(PingMessage message);
    }
}
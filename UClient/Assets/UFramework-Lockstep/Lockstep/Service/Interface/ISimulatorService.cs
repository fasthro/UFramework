/*
 * @Author: fasthro
 * @Date: 2020/12/30 19:38:57
 * @Description:
 */
using Lockstep.MessageData;

namespace Lockstep
{
    public interface ISimulatorService : IService
    {
        void Start(GameStart data);
        void OnReceiveFrame(Frame frame);
        void OnReceiveFrames(Frame[] frame);
    }
}
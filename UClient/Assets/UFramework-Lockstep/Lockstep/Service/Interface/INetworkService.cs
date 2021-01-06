/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:07:48
 * @Description:
 */
using Lockstep.MessageData;

namespace Lockstep
{
    public interface INetworkService : IService
    {
        void SendFrame(Frame frame);
    }
}
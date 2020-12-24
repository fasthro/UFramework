/*
 * @Author: fasthro
 * @Date: 2020/12/22 10:32:52
 * @Description:
 */

using Google.Protobuf;

namespace LockstepServer
{
    public interface IHandler : IUdpSocketHandler
    {
        int cmd { get; }
        int session { get; set; }
        NetworkProcessLayer layer { get; set; }
        IMessage responseMessage { get; set; }

        bool DoResponse();

        void DoAction();
    }
}
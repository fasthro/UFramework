/*
 * @Author: fasthro
 * @Date: 2020/12/22 10:32:52
 * @Description:
 */

namespace LockstepServer
{
    public interface IHandler : IUdpSocketHandler
    {
        int cmd { get; }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:49:52
 * @Description:
 */

namespace LockstepServer.Src
{
    public interface IRoomService : IService
    {
        Room room { get; }
    }
}
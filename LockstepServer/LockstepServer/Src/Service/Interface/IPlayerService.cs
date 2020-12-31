/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:49:52
 * @Description:
 */

namespace LockstepServer.Src
{
    public interface IPlayerService : IService
    {
        Player GetPlayer(int sessionId);

        void AddPlayer(Player player);
    }
}
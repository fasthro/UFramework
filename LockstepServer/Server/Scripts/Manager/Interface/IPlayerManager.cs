// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using UFramework;

namespace GameServer
{
    public interface IPlayerManager : IManager
    {
        Player GetPlayer(long uid);
        Player GetPlayer(int sessionId);

        void AddPlayer(Player player);

        void RemovePlayer(long uid);

        bool ExistPlayer(long uid);
    }
}
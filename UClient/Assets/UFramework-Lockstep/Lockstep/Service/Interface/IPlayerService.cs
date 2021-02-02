// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public interface IPlayerService : IService
    {
        Player self { get; }
        void AddPlayer(GameEntity entity, PlayerData player);
        Player GetPlayer(int oid);
        Player GetPlayer(long uid);
    }
}
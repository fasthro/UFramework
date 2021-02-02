// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public class GameService : BaseService, IGameService
    {
        public long uid { get; set; }
        public int oid { get; set; }

        public bool IsSelf(int localId)
        {
            return this.oid == localId;
        }

        public bool IsSelf(long userId)
        {
            return this.uid == userId;
        }
    }
}
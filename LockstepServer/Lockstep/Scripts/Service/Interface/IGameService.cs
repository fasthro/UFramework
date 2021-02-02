// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public interface IGameService : IService
    {
        long uid { get; set; }
        int oid { get; set; }

        bool IsSelf(int oid);
        bool IsSelf(long uid);
    }
}
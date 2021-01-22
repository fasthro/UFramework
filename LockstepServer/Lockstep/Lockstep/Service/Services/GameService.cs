/*
 * @Author: fasthro
 * @Date: 2020/12/30 18:08:16
 * @Description:
 */

using System;
namespace Lockstep
{
    public class GameService : BaseService, IGameService
    {
        public long userId { get; set; }
        public int localId { get; set; }

        public bool IsSelf(int localId)
        {
            return this.localId == localId;
        }

        public bool IsSelf(long userId)
        {
            return this.userId == userId;
        }
    }
}

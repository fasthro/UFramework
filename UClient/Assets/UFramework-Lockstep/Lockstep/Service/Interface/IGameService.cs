/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:31:41
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockstep.MessageData;

namespace Lockstep
{
    public interface IGameService : IService
    {
        long userId { get; set; }
        int localId { get; set; }

        bool IsSelf(int localId);
        bool IsSelf(long userId);
    }
}
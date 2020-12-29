/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:32:13
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockstep
{
    public abstract class BaseService : IService
    {
        protected GameService _gameService;
    }
}
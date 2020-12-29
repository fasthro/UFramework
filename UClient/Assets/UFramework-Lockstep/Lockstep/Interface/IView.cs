/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:36:58
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockstep
{
    public interface IView
    {
        void BindEntity(GameEntity entity);
    }
}
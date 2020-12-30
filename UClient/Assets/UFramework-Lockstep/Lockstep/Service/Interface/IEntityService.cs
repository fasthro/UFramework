/*
 * @Author: fasthro
 * @Date: 2020/12/30 19:38:18
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockstep
{
    public interface IEntityService : IService
    {
        GameEntity AddEntity<T>(Contexts contexts, T view) where T : IView;

        List<GameEntity> GetEntities<T>() where T : IView;

        void RemoveEntity<T>(GameEntity entity) where T : IView;
    }
}
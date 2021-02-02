// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/30 19:38:18
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Lockstep
{
    public interface IEntityService : IService
    {
        GameEntity AddEntity<T>(Contexts contexts, T view) where T : IView;

        List<GameEntity> GetEntities<T>() where T : IView;

        void RemoveEntity<T>(GameEntity entity) where T : IView;
    }
}
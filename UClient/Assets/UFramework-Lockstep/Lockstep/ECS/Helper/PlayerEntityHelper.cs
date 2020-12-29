/*
 * @Author: fasthro
 * @Date: 2020/12/29 17:17:50
 * @Description:
 */

namespace Lockstep
{
    public static class PlayerEntityHelper
    {
        public static GameEntity Create(Contexts contexts, IView view)
        {
            var entity = contexts.game.CreateEntity();
            entity.AddCView(view);
            view.BindEntity(entity);
            return entity;
        }
    }
}
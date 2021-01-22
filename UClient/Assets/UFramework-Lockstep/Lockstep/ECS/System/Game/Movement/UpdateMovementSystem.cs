/*
 * @Author: fasthro
 * @Date: 2020/12/30 16:02:47
 * @Description:
 */

using Entitas;
using System.Collections.Generic;

namespace Lockstep
{
    public class UpdateMovementSystem : ReactiveSystem<GameEntity>
    {
        public UpdateMovementSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                var np = entity.cPosition.value + entity.cMovement.inputDirection.normalized * LSTime.deltaTime * entity.cSpeed.value;
                entity.ReplaceCPosition(np);
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCMovement && entity.hasCPosition && entity.hasCSpeed;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.CMovement, GameMatcher.CPosition, GameMatcher.CSpeed));
        }
    }
}
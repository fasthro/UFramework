using System.Collections.Generic;
using Entitas;

namespace Lockstep
{
    public class UpdateViewPositionSystem : ReactiveSystem<GameEntity>
    {
        public UpdateViewPositionSystem(Contexts contexts) : base(contexts.game)
        {
        }

        public UpdateViewPositionSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.CPosition));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCPosition;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
                entity.cView.value.position = entity.cPosition.value;
        }
    }
}
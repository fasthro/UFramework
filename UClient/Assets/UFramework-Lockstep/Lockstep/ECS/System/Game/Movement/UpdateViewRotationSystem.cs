using System.Collections.Generic;
using Entitas;

namespace Lockstep
{
    public class UpdateViewRotationSystem : ReactiveSystem<GameEntity>
    {
        public UpdateViewRotationSystem(Contexts contexts) : base(contexts.game)
        {
        }

        public UpdateViewRotationSystem(ICollector<GameEntity> collector) : base(collector)
        {
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.CRotation));
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCRotation;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
                entity.cView.value.deg = entity.cRotation.value;
        }
    }
}
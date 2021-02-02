using System.Collections.Generic;
using Entitas;

namespace Lockstep
{
    public class UpdateViewSystem : ReactiveSystem<GameEntity>
    {
        public UpdateViewSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
                entity.cView.value.Update();
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCView;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.CView));
        }
    }
}
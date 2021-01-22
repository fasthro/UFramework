/*
 * @Author: fasthro
 * @Date: 2020/12/30 16:02:47
 * @Description:
 */

using Entitas;
using System.Collections.Generic;
using System.Numerics;

namespace Lockstep
{
    public class UpdateViewPositionAndRotationSystem : ReactiveSystem<GameEntity>
    {
        public UpdateViewPositionAndRotationSystem(Contexts contexts) : base(contexts.game)
        {
            _context = contexts.game;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.cView.view.position = entity.cView.view.position + Vector3.Normalize(entity.cPosition.position) * 1f;
                entity.cView.view.rotation = entity.cRotation.rotation;
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCPosition && entity.hasCRotation;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(GameMatcher.CPosition, GameMatcher.CRotation));
        }

        private readonly GameContext _context;
    }
}
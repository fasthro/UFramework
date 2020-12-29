using Entitas;
using UnityEngine;

namespace Lockstep
{
    public class MoveSystem : IExecuteSystem
    {
        public MoveSystem(Contexts contexts)
        {
            _group = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.CTransform));
        }

        public void Execute()
        {
            var entitys = _group.GetEntities();
            foreach (var entity in entitys)
            {
                entity.cTransform.position = Vector3.one;
            }
        }

        private readonly IGroup<GameEntity> _group;
    }
}
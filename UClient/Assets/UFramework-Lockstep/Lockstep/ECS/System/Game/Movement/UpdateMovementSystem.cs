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
        private LSVector2 _degV2;

        public UpdateMovementSystem(Contexts contexts) : base(contexts.game)
        {
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                var need = LSVector3.SqrMagnitude(entity.cMovement.inputDirection) > Fix64.Zero;
                if (need)
                {
                    var dir = entity.cMovement.inputDirection.normalized;

                    // position
                    var newPosition = entity.cPosition.value + dir * LSTime.deltaTime * entity.cMoveSpeed.value;
                    entity.ReplaceCPosition(newPosition);

                    // rotation
                    _degV2.x = dir.x;
                    _degV2.y = dir.z;
                    var targetDeg = LSMath.ToDeg(_degV2);
                    var newDeg = LSMath.TurnToward(targetDeg, entity.cRotation.value, LSMath.Value360 * entity.cRotationSpeed.value, out var hasReachDeg);
                    entity.ReplaceCRotation(newDeg);
                    Logger.Debug(LSMath.Value360 * entity.cRotationSpeed.value);
                }
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCMovement && entity.hasCPosition && entity.hasCRotation && entity.hasCMoveSpeed && entity.hasCRotationSpeed;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(
                GameMatcher.AllOf(
                    GameMatcher.CMovement,
                    GameMatcher.CPosition,
                    GameMatcher.CRotation,
                    GameMatcher.CMoveSpeed,
                    GameMatcher.CRotationSpeed));
        }
    }
}
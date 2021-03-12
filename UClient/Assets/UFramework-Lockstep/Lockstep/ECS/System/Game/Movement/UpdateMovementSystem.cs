// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/30 16:02:47
// * @Description:
// --------------------------------------------------------------------------------

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
                if (!entity.cMovement.isMission)
                {
                    var isMove = entity.cMovement.dir.sqrMagnitude != FP.Zero;
                    if (isMove)
                    {
                        var dir = entity.cMovement.dir.normalized;
                        // position
                        entity.cTransform.position += dir * LSTime.tickDeltaTime * entity.cSpeed.moveSpeed;
                        

                        // target rotation
                        var ccwDeg = LSMath.Atan2(dir.z, dir.x) * LSMath.Rad2Deg;
                        var deg = (90 - ccwDeg) % 360;
                        entity.cTransform.targetRotation = LSQuaternion.Euler(0, deg, 0);

                        // flag
                        entity.cMovement.isMission = true;
                    }

                    entity.cTransform.rotation = LSQuaternion.RotateTowards(entity.cTransform.rotation, entity.cTransform.targetRotation, LSTime.tickDeltaTime * entity.cSpeed.rotationSpeed);
                }
            }
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasCMovement && entity.hasCTransform && entity.hasCSpeed;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(
                GameMatcher.AllOf(
                    GameMatcher.CMovement,
                    GameMatcher.CTransform,
                    GameMatcher.CSpeed));
        }
    }
}
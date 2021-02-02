// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 17:35
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public class Player : BaseAgent, IPoolBehaviour
    {
        public PlayerData playerData { get; private set; }

        #region pool

        public bool isRecycled { get; set; }

        public static Player Allocate(GameEntity entity, PlayerData playerData)
        {
            return ObjectPool<Player>.Instance.Allocate().Builder(entity, playerData);
        }

        public void Recycle()
        {
            ObjectPool<Player>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
        }

        #endregion

        private Player Builder(GameEntity entity, PlayerData playerData)
        {
            this.entity = entity;
            this.playerData = playerData;
            return this;
        }
    }
}
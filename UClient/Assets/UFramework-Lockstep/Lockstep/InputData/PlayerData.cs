// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 16:22
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public class PlayerData: IPoolBehaviour
    {
        public long uid;
        public int oid;
        public string name;
        
        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<PlayerData>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
        }

        #endregion
    }
}
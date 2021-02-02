// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 16:16
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public class InputData : IPoolBehaviour
    {
        public int oid;
        public LSVector3 movementDir;

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<InputData>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
        }

        #endregion
    }
}
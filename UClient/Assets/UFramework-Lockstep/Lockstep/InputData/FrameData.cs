// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 16:24
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Lockstep
{
    public class FrameData : IPoolBehaviour
    {
        public int tick;
        public InputData[] inputDatas;

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<FrameData>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
        }

        #endregion

        public static bool CheckEquals(FrameData x, FrameData y)
        {
            return true;
        }
    }
}
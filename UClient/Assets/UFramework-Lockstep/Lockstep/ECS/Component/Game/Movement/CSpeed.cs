// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 14:26
// * @Description:
// --------------------------------------------------------------------------------

using Entitas;

namespace Lockstep
{
    public class CSpeed : IComponent
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public FP moveSpeed;
        
        /// <summary>
        /// 旋转速度
        /// </summary>
        public FP rotationSpeed;
    }
}
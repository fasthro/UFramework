using Entitas;

namespace Lockstep
{
    public class CRotation : IComponent
    {
        /// <summary>
        /// 目标角度角度
        /// </summary>
        public FP target;
        
        /// <summary>
        /// 角度
        /// </summary>
        public FP value;
    }
}
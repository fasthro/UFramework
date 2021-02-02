using Entitas;

namespace Lockstep
{
    public class CMovement : IComponent
    {
        /// <summary>
        /// 输入方向
        /// </summary>
        public LSVector3 dir;
        
        /// <summary>
        /// 移动 Miss
        /// </summary>
        public bool isMission;
    }
}
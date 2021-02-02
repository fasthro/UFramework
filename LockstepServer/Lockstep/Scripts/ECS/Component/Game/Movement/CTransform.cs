// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 14:00
// * @Description:
// --------------------------------------------------------------------------------

using Entitas;

namespace Lockstep
{
    public class CTransform : IComponent
    {
        public LSVector3 position;
        public LSQuaternion rotation;
        public LSQuaternion targetRotation;
    }
}
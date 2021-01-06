/*
 * @Author: fasthro
 * @Date: 2020/12/29 17:29:26
 * @Description:
 */

using System.Numerics;
using Entitas;

namespace Lockstep
{
    [Game]
    public class CRotation : IComponent
    {
        public Quaternion rotation;
    }
}
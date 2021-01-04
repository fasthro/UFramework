/*
 * @Author: fasthro
 * @Date: 2020/12/29 17:29:26
 * @Description:
 */

using Entitas;
using System.Numerics;

namespace Lockstep
{
    [Game]
    public class CPosition : IComponent
    {
        public Vector3 position;
    }
}
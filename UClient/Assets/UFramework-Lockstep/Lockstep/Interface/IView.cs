/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:36:58
 * @Description:
 */

using System.Numerics;

namespace Lockstep
{
    public interface IView
    {
        Vector3 position { get; set; }
        Quaternion rotation { get; set; }

        int localID { get; set;}
        GameEntity entity { get; }

        void BindEntity(GameEntity entity);
    }
}
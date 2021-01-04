/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:39:13
 * @Description:
 */

using System.Numerics;

namespace Lockstep
{
    public class BaseView : IView
    {
        public Vector3 position { get; set; }

        public Quaternion rotation { get; set; }

        public GameEntity entity => _entity;

        public void BindEntity(GameEntity entity)
        {
            _entity = entity;
        }

        protected GameEntity _entity;
    }
}
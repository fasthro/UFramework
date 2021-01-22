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

        public int localID { get; set; }

        public GameEntity entity => _entity;

        protected GameEntity _entity;

        public void BindEntity(GameEntity entity)
        {
            _entity = entity;
        }

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}
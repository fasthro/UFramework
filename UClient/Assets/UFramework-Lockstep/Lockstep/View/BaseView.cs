/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:39:13
 * @Description:
 */

namespace Lockstep
{
    public abstract class BaseView : IView
    {
        #region public

        public LSVector3 position => _entity.cTransform.position;
        public LSQuaternion rotation => _entity.cTransform.rotation;

        public GameEntity entity => _entity;

        #endregion

        #region private

        private GameEntity _entity;

        #endregion

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
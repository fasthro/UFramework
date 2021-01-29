/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:39:13
 * @Description:
 */

namespace Lockstep
{
    public class BaseView : IView
    {
        #region public

        public LSVector3 position { get; set; }
        public FP deg { get; set; }
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
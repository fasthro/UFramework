// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/30 14:57:27
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace Lockstep.Logic
{
    public abstract class BaseGameView : MonoBehaviour, IView
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
            transform.position = Vector3.Lerp(transform.position, position.ToVector3(), Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation.ToQuaternion(), Time.deltaTime * 10f);
            
            OnUpdate();
        }


        protected virtual void OnUpdate()
        {
        }
    }
}
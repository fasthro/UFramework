/*
 * @Author: fasthro
 * @Date: 2020/12/30 14:57:27
 * @Description:
 */

using UnityEngine;

namespace Lockstep.Logic
{
    public class BaseGameView : MonoBehaviour, IView
    {
        #region public

        public LSVector3 position
        {
            get => transform.position.ToLSVector3();
            set => _position = value;
        }

        public FP deg
        {
            get => (FP) transform.localEulerAngles.y;
            set => _deg = value;
        }

        public GameEntity entity => _entity;

        #endregion

        #region private

        private GameEntity _entity;
        private LSVector3 _position;
        private FP _deg;

        #endregion

        public void BindEntity(GameEntity entity)
        {
            _entity = entity;
        }

        public void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _position.ToVector3(), 0.3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, (float) _deg, 0), 0.3f);
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}
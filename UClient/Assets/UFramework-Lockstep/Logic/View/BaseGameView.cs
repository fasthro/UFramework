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
            get => entity.cPosition.value;
            set => _tPos = value.ToVector3();
        }

        public GameEntity entity => _entity;

        #endregion

        #region private

        private GameEntity _entity;
        private Vector3 _tPos;

        #endregion

        public void BindEntity(GameEntity entity)
        {
            _entity = entity;
        }

        public void Update()
        {
            Vector3.MoveTowards(transform.position, _tPos, Time.deltaTime);
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}
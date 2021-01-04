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
        public Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        public Quaternion rotation
        {
            get { return transform.rotation; }
            set { transform.rotation = value; }
        }

        public GameEntity entity => _entity;

        public void BindEntity(GameEntity entity)
        {
            _entity = entity;
        }

        protected GameEntity _entity;
    }
}
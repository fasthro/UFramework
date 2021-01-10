/*
 * @Author: fasthro
 * @Date: 2020/12/30 14:57:27
 * @Description:
 */

using UnityEngine;

using V3 = System.Numerics.Vector3;
using QR = System.Numerics.Quaternion;
using UV3 = UnityEngine.Vector3;
using UQR = UnityEngine.Quaternion;

namespace Lockstep.Logic
{
    public class BaseGameView : MonoBehaviour, IView
    {
        public V3 position
        {
            get { return transform.position.ToCS(); }
            set
            {
                Debug.Log("position: " + value.ToString());
                transform.position = value.ToUnity();
            }
        }

        public QR rotation
        {
            get { return transform.rotation.ToCS(); }
            set { transform.rotation = value.ToUnity(); }
        }
        public int localID { get; set; }

        public GameEntity entity => _entity;

        public void BindEntity(GameEntity entity)
        {
            _entity = entity;
        }

        protected GameEntity _entity;
    }
}
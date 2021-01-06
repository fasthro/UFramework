/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:22:03
 * @Description: 
 */

using System.Numerics;
using Google.Protobuf;

namespace Lockstep.MessageData
{
    public class AgentInput : BaseMessageData, IPoolBehaviour
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// 旋转
        /// </summary>
        public Quaternion rotation;

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<AgentInput>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }

        #endregion

        #region message

        public override object ToMessage()
        {
            var msg = new PBBSCommon.Input();
            msg.Px = position.X;
            msg.Py = position.Y;
            msg.Pz = position.Z;
            return msg;
        }

        public override void FromMessage(IMessage message)
        {
            var msg = (PBBSCommon.Input)message;
            position.X = (float)msg.Px;
            position.Y = (float)msg.Py;
            position.Z = (float)msg.Pz;
        }
        #endregion

        #region clone
        public override object DeepClone()
        {
            var co = ObjectPool<AgentInput>.Instance.Allocate();
            co.position = new Vector3(position.X, position.Y, position.Z);
            return co;
        }
        #endregion
    }
}
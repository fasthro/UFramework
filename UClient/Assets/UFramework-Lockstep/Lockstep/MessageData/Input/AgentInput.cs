/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:22:03
 * @Description: 
 */

using Google.Protobuf;

namespace Lockstep.MessageData
{
    public class AgentInput : BaseMessageData, IPoolBehaviour
    {
        /// <summary>
        /// 位置
        /// </summary>
        public LSVector3 inputDirection;

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
            return new PBBSCommon.Input
            {
                Px = (double) inputDirection.x, Py = (double) inputDirection.y, Pz = (double) inputDirection.z
            };
        }

        public override void FromMessage(IMessage message)
        {
            var msg = (PBBSCommon.Input) message;
            inputDirection.x = (FP) msg.Px;
            inputDirection.y = (FP) msg.Py;
            inputDirection.z = (FP) msg.Pz;
        }

        #endregion

        #region clone

        public override object DeepClone()
        {
            var co = ObjectPool<AgentInput>.Instance.Allocate();
            co.inputDirection = new LSVector3(inputDirection.x, inputDirection.y, inputDirection.z);
            return co;
        }

        #endregion
    }
}
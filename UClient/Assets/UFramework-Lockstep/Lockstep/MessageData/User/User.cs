/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:22:03
 * @Description: 
 */

using Google.Protobuf;

namespace Lockstep.MessageData
{
    public class User : BaseMessageData, IPoolBehaviour
    {
        /// <summary>
        /// user id
        /// </summary>
        public long id;

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<User>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }

        #endregion

        #region message

        public override object ToMessage()
        {
            var msg = new PBBSCommon.User();
            msg.Id = id;
            return msg;
        }

        public override void FromMessage(IMessage message)
        {
            var msg = (PBBSCommon.User)message;
            id = msg.Id;
        }

        public override object DeepClone()
        {
            return Clone();
        }

        #endregion
    }
}
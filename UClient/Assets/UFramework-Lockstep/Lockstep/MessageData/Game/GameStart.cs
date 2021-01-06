/*
 * @Author: fasthro
 * @Date: 2020/12/31 14:28:42
 * @Description:
 */

using Google.Protobuf;

namespace Lockstep.MessageData
{
    public class GameStart : BaseMessageData, IPoolBehaviour
    {
        /// <summary>
        /// 统一随机种子
        /// </summary>
        public int randSeed;

        /// <summary>
        /// 用户数据
        /// </summary>
        public User[] users;

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<GameStart>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {

        }

        #endregion

        #region message

        public override object ToMessage()
        {
            return null;
        }

        public override void FromMessage(IMessage message)
        {
            
        }

        public override object DeepClone()
        {
            return Clone();
        }
        #endregion
    }
}
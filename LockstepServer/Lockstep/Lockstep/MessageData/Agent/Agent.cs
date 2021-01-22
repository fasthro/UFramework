/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:22:03
 * @Description: 
 */

using System.Collections.Generic;
using Google.Protobuf;

namespace Lockstep.MessageData
{
    public class Agent : BaseMessageData, IPoolBehaviour
    {
        /// <summary>
        /// 本地ID
        /// </summary>
        public int localId;

        /// <summary>
        /// 输入列表
        /// </summary>
        public readonly List<AgentInput> inputs = new List<AgentInput>();

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<Agent>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            for (var i = 0; i < inputs.Count; i++)
                inputs[i].Recycle();
        }

        #endregion

        #region message

        public override object ToMessage()
        {
            var msg = new PBBSCommon.Agent {LocalId = localId};
            for (var i = 0; i < inputs.Count; i++)
                msg.Inputs.Add((PBBSCommon.Input) inputs[i].ToMessage());
            return msg;
        }

        public override void FromMessage(IMessage message)
        {
            var msg = (PBBSCommon.Agent) message;
            localId = msg.LocalId;

            inputs.Clear();
            for (var i = 0; i < msg.Inputs.Count; i++)
            {
                var input = ObjectPool<AgentInput>.Instance.Allocate();
                input.FromMessage(msg.Inputs[i]);
                inputs.Add(input);
            }
        }

        #endregion

        #region clone

        public override object DeepClone()
        {
            var co = ObjectPool<Agent>.Instance.Allocate();
            co.localId = localId;
            for (int i = 0; i < inputs.Count; i++)
                co.inputs.Add((AgentInput) inputs[i].DeepClone());
            return co;
        }

        #endregion
    }
}
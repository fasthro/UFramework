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
        /// 输入
        /// </summary>
        public AgentInput[] inputs => _inputs.ToArray();

        public GameEntity entity;

        public IView view;

        #region input

        private List<AgentInput> _inputs = new List<AgentInput>();

        public void AddInput(AgentInput input)
        {
            _inputs.Add(input);
        }

        public void CleanInputs()
        {
            _inputs.Clear();
        }

        #endregion

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<Agent>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            for (int i = 0; i < _inputs.Count; i++)
                inputs[i].Recycle();
        }

        #endregion

        #region message

        public override object ToMessage()
        {
            var msg = new PBBSCommon.Agent();
            msg.LocalId = localId;
            for (int i = 0; i < _inputs.Count; i++)
                msg.Inputs.Add((PBBSCommon.Input)_inputs[i].ToMessage());
            return msg;
        }

        public override void FromMessage(IMessage message)
        {
            var msg = (PBBSCommon.Agent)message;
            localId = msg.LocalId;

            CleanInputs();
            for (int i = 0; i < msg.Inputs.Count; i++)
            {
                var input = ObjectPool<AgentInput>.Instance.Allocate();
                input.FromMessage(msg.Inputs[i]);
                AddInput(input);
            }
        }
        #endregion

        #region clone
        public override object DeepClone()
        {
            var co = ObjectPool<Agent>.Instance.Allocate();
            co.localId = localId;
            for (int i = 0; i < _inputs.Count; i++)
                co.AddInput((AgentInput)_inputs[i].DeepClone());
            return co;
        }
        #endregion
    }
}
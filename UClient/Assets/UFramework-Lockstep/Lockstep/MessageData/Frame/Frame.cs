/*
 * @Author: fasthro
 * @Date: 2021-01-04 15:22:03
 * @Description: 
 */

using System.Collections.Generic;
using Google.Protobuf;

namespace Lockstep.MessageData
{
    public class Frame : BaseMessageData, IPoolBehaviour
    {
        /// <summary>
        /// 当前帧
        /// </summary>
        public int tick;

        /// <summary>
        /// 代理
        /// </summary>
        public Agent[] agents => _agents.ToArray();

        #region agents

        private List<Agent> _agents = new List<Agent>();

        public void AddAgent(Agent agent)
        {
            _agents.Add(agent);
        }

        public void AddAgents(IEnumerable<Agent> agent)
        {
            _agents.AddRange(agent);
        }

        #endregion

        #region pool

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<Frame>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            foreach (var agent in agents)
                agent.Recycle();
        }

        #endregion

        #region message

        public override object ToMessage()
        {
            var msg = new PBBSCommon.Frame();
            msg.Tick = tick;

            for (int i = 0; i < agents.Length; i++)
                msg.Agents.Add((PBBSCommon.Agent)agents[i].ToMessage());
            return msg;
        }

        public override void FromMessage(IMessage message)
        {
            var msg = (PBBSCommon.Frame)message;
            tick = msg.Tick;

            _agents.Clear();
            var agnetCount = msg.Agents.Count;
            for (int i = 0; i < agnetCount; i++)
            {
                var agent = ObjectPool<Agent>.Instance.Allocate();
                agent.FromMessage(msg.Agents[i]);
                AddAgent(agent);
            }
        }
        #endregion

        #region clone
        public override object DeepClone()
        {
            var co = ObjectPool<Frame>.Instance.Allocate();
            co.tick = tick;
            for (int i = 0; i < _agents.Count; i++)
                co.AddAgent((Agent)_agents[i].DeepClone());
            return co;
        }
        #endregion
    }
}
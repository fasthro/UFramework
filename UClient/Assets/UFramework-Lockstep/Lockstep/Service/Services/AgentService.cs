/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:31:57
 * @Description:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Lockstep.MessageData;

namespace Lockstep
{
    public class AgentService : BaseService, IAgentService
    {
        public Agent selfAgent => _selfAgent;
        private Agent _selfAgent;

        public Agent[] agents => _agents;
        private Agent[] _agents;

        public override void Initialize()
        {
            _agents = new Agent[Define.MAX_PLAYER_COUNT];
        }

        public void CreateAgent(GameEntity entity)
        {
            var localId = entity.cLocalId.id;

            var agent = _agents[localId] = ObjectPool<Agent>.Instance.Allocate();
            agent.localId = localId;
            agent.entity = entity;
            if (_gameService.IsSelf(localId))
            {
                _selfAgent = agent;
            }
        }

        public Agent GetAgent(int localId)
        {
            return _agents[localId];
        }
    }
}
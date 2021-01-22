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
        #region public

        public Agent[] agents => _agents;
        public Agent self => _self;

        #endregion

        #region private

        private Agent[] _agents;
        private Agent _self;

        #endregion

        public override void Initialize()
        {
            _agents = new Agent[Define.MAX_PLAYER_COUNT];
        }

        public void CreateAgent(GameEntity entity)
        {
            var localId = entity.cLocalId.value;
            var agent = agents[localId] = ObjectPool<Agent>.Instance.Allocate();
            agent.localId = localId;

            if (_gameService.IsSelf(localId))
                _self = agent;
        }

        public Agent GetAgent(int localId)
        {
            return agents[localId];
        }
    }
}
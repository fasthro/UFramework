/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:31:41
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lockstep.MessageData;

namespace Lockstep
{
    public interface IAgentService : IService
    {
        /// <summary>
        /// 自己
        /// </summary>
        Agent self { get; }
        
        /// <summary>
        /// 代理列表
        /// </summary>
        Agent[] agents { get; }
        
        /// <summary>
        /// 创建代理
        /// </summary>
        /// <param name="entity"></param>
        void CreateAgent(GameEntity entity);
        
        /// <summary>
        /// 获取代理
        /// </summary>
        /// <param name="localId"></param>
        /// <returns></returns>
        Agent GetAgent(int localId);
    }
}
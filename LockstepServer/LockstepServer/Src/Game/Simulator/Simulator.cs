/*
 * @Author: fasthro
 * @Date: 2020/12/24 14:41:37
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    public class Simulator : BaseBehaviour
    {
        /// <summary>
        /// 随机种子
        /// </summary>
        public int seed { get; private set; }

        /// <summary>
        /// 当前逻辑帧
        /// </summary>
        public int tick { get; private set; }

        protected override void OnInitialize()
        {
            seed = new Random().Next();
            LogHelper.Debug($"Random Seed:{seed}");
        }

        protected override void OnUpdate(float deltaTime)
        {
            tick++;
        }
    }
}
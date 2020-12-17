/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:32:45
 * @Description: 帧同步模拟器服务
 */
using UnityEngine;

namespace UFramework.Lockstep
{
    public class SimulatorService : BaseService
    {
        /// <summary>
        /// 客户端模式
        /// </summary>
        /// <value></value>
        public bool isClientMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool isRunning { get; private set; }

        /// <summary>
        /// 当前逻辑帧
        /// </summary>
        public int tick { get; private set; }

        /// <summary>
        /// 帧记录
        /// </summary>
        /// <value></value>
        public int tickSinceStart { get; private set; }

        /// <summary>
        /// 当前预测帧
        /// </summary>
        public int predictTick { get; private set; }

        /// <summary>
        /// 当前服务器帧
        /// </summary>
        public int serverTick { get; private set; }

        /// <summary>
        /// 每帧刷新时间
        /// </summary>
        /// <value></value>
        public long tickDeltaTime { get; private set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <value></value>
        public long startTime { get; private set; }


        public override void OnAwake()
        {
            tickDeltaTime = 1000 / LSDefine.FRAME_RATE;
        }

        public void StartSimulate()
        {
            tick = 0;
            startTime = LSTime.realtimeSinceStartupMS;
            isRunning = true;
            isClientMode = true;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!isRunning) return;

            tickSinceStart = (int)((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);

            if (isClientMode) UpdateClient();
            else UpdateServer();
        }

        /// <summary>
        /// 客户端模式更新
        /// </summary>
        private void UpdateClient()
        {
            while (tick < tickSinceStart)
            {
                tick++;
                Simulate();
            }
        }

        /// <summary>
        /// 服务器模式更新
        /// </summary>
        private void UpdateServer()
        {

        }

        /// <summary>
        /// 模拟
        /// </summary>
        private void Simulate()
        {

        }

        /// <summary>
        /// 预测
        /// </summary>
        private void Predict()
        {

        }

        private void Step()
        {

        }
    }
}
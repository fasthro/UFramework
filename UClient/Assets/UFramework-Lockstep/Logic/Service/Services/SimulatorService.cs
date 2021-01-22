﻿/*
 * @Author: fasthro
 * @Date: 2020/12/31 11:55:03
 * @Description:
 */

using Lockstep.MessageData;

namespace Lockstep.Logic
{
    public class SimulatorService : BaseGameBehaviour, ISimulatorService
    {
        #region simulator

        public Simulator simulator { get; private set; }

        /// <summary>
        /// 客户端模式
        /// </summary>
        public bool isClientModel = false;

        /// <summary>
        /// 帧记录
        /// </summary>
        /// <value></value>
        public int tickSinceStart { get; private set; }

        /// <summary>
        /// 输入帧
        /// </summary>
        /// <value></value>
        public int inputTick { get; private set; }

        /// <summary>
        /// 预测帧
        /// </summary>
        public int inputPredictTick => tickSinceStart + 1;

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

        #endregion simulator

        #region data

        public GameStart gameStart { get; private set; }
        public GameEntity[] players { get; private set; }

        #endregion data

        private FrameBuffer _frameBuffer;

        public void Start(GameStart data)
        {
            Logger.Debug("开始游戏");
            gameStart = data;
            tickDeltaTime = 1000 / Define.FRAME_RATE;
            _frameBuffer = new FrameBuffer();
            simulator = new Simulator();
            simulator.Start(data);

            var entitys = _entityService.GetEntities<IPlayerView>();
            players = new GameEntity[entitys.Count];
            foreach (var entity in entitys)
                players[entity.cLocalId.value] = entity;
        }

        public override void Update()
        {
            if (simulator == null || !simulator.isRunning) return;

            simulator.Update();

            tickSinceStart = (int) ((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);
            if (isClientModel)
            {
                while (simulator.tick < tickSinceStart)
                {
                    var frame = _frameBuffer.GetFrame(simulator.tick);
                    if (frame != null)
                    {
                        SimulateStep(frame);
                    }
                }
            }
            else
            {
                while (inputTick <= inputPredictTick)
                {
                    SendInputs(inputTick++);
                }

                UpdateServer();
            }
        }

        private void UpdateServer()
        {
            while (simulator.tick < _frameBuffer.sTick)
            {
                var frame = _frameBuffer.GetFrame(simulator.tick + 1);
                if (frame != null)
                {
                    SimulateStep(frame);
                }
            }
        }

        private void SimulateStep(Frame frame)
        {
            ProcessFrame(frame);
            simulator.Step();
        }

        private void ProcessFrame(Frame frame)
        {
            foreach (var agent in frame.agents)
            {
                if (agent.inputs.Count <= 0) continue;
                foreach (var input in agent.inputs)
                    _inputService.ExecuteInput(players[agent.localId], input);
            }
        }

        private void SendInputs(int tick)
        {
            var frame = ObjectPool<Frame>.Instance.Allocate();
            frame.tick = tick;
            frame.AddAgents(_agentService.agents);
            _networkService.SendFrame(frame);
            _agentService.self.inputs.Clear();
        }

        public void OnReceiveFrame(Frame frame)
        {
            _frameBuffer.PushSFrame(frame);
        }

        public void OnReceiveFrames(Frame[] frames)
        {
            for (var i = 0; i < frames.Length; i++)
                _frameBuffer.PushSFrame(frames[i]);
        }
    }
}
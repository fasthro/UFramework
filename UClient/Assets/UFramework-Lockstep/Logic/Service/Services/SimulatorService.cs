// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/31 11:55:03
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UFramework.Core;
using UnityEngine;

namespace Lockstep.Logic
{
    public class SimulatorService : BaseGameBehaviour, ISimulatorService
    {
        #region simulator

        /// <summary>
        /// 客户端模式
        /// </summary>
        public bool isClientModel = false;

        /// <summary>
        /// 运行中
        /// </summary>
        public bool isRunning { get; private set; }

        /// <summary>
        /// 当前逻辑镇
        /// </summary>
        public int tick { get; private set; }

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

        #region Ping

        /// <summary>
        /// Ping
        /// </summary>
        public int pingValue { get; private set; }

        /// <summary>
        /// Max Ping
        /// </summary>
        public long maxPingValue { get; private set; }

        /// <summary>
        /// Min Ping
        /// </summary>
        public long minPingValue { get; private set; }

        /// <summary>
        /// 延迟
        /// </summary>
        public int delayValue { get; private set; }


        #region private

        private float _pingTime;
        private List<long> _pings = new List<long>();
        private List<long> _delays = new List<long>();

        #endregion

        #endregion


        private FrameBuffer _frameBuffer;

        private void InitializeSimulator(GameStartMessage message)
        {
            tickDeltaTime = 1000 / LSDefine.FRAME_RATE;
            _frameBuffer = new FrameBuffer();
            isRunning = true;

            // 添加玩家
            foreach (var playerData in message.playerDatas)
            {
                var view = _viewService.CreateView<IPlayerView>("Assets/Arts/Player/Player1.prefab", playerData.oid);
                // TODO
                if (playerData.uid == 1)
                {
                    _gameService.uid = playerData.uid;
                    _gameService.oid = playerData.oid;
                }

                _playerService.AddPlayer(view.entity, playerData);
            }

            // 广播游戏初始化
            Messenger.Broadcast(EventDefine.GAME_INIT);

            // 广播游戏开始
            Messenger.Broadcast(EventDefine.GAME_START);
        }

        public override void Update()
        {
            if (!isRunning) return;

            #region ping && delay

            _networkService.SendPing();

            _pingTime += Time.deltaTime;
            if (_pingTime > LSDefine.PING_TIME)
            {
                _pingTime = 0;
                
                // ping
                pingValue = (int) _pings.Sum() / LSMath.Max(_pings.Count, 1);
                _pings.Clear();

                // delay
                delayValue = (int) _delays.Sum() / LSMath.Max(_delays.Count, 1);
                _delays.Clear();

                _uiService.UpdatePing(pingValue);
                _uiService.UpdateDelay(delayValue);
            }

            #endregion


            tickSinceStart = (int) ((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);
            if (isClientModel)
            {
                while (tick < tickSinceStart)
                {
                    var frame = _frameBuffer.GetFrame(tick);
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
            while (tick < _frameBuffer.sTick)
            {
                var frame = _frameBuffer.GetFrame(tick + 1);
                if (frame != null)
                {
                    SimulateStep(frame);
                }
            }
        }

        private void SimulateStep(FrameData frame)
        {
            ProcessFrame(frame);
            tick++;
        }

        private void ProcessFrame(FrameData frame)
        {
            foreach (var input in frame.inputDatas)
            {
                var player = _playerService.GetPlayer(input.oid);
                player?.entity.ReplaceCMovement(input.movementDir, false);
            }
        }

        private void SendInputs(int tick)
        {
            var frame = ObjectPool<FrameData>.Instance.Allocate();
            frame.tick = tick;
            frame.inputDatas = new[] {_inputService.inputData};
            _networkService.SendInput(frame);
        }

        #region frame

        private void PushSFrame(FrameData frameData)
        {
            _frameBuffer.PushSFrame(frameData);
        }

        #endregion

        #region receive message

        public void OnReceiveGameStart(GameStartMessage message)
        {
            InitializeSimulator(message);
        }

        public void OnReceiveFrame(FrameData frame)
        {
            PushSFrame(frame);
        }

        public void OnReceivePing(PingMessage message)
        {
            var ping = LSTime.realtimeSinceStartupMS - message.sendTimestamp;
            _pings.Add(ping);

            if (ping > maxPingValue)
                maxPingValue = ping;

            if (ping < minPingValue)
                minPingValue = ping;
        }

        #endregion
    }
}
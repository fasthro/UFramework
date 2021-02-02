/*
 * @Author: fasthro
 * @Date: 2020/12/31 11:55:03
 * @Description:
 */

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


        private FrameBuffer _frameBuffer;

        private void InitializeSimulator(GameStartMessage message)
        {
            tickDeltaTime = 1000 / Define.FRAME_RATE;
            _frameBuffer = new FrameBuffer();
            isRunning = true;

            // 添加玩家
            IView self = null;
            foreach (var playerData in message.playerDatas)
            {
                var view = _viewService.CreateView<IPlayerView>("Assets/Arts/Player/Player1.prefab", playerData.oid);
                // TODO
                if (playerData.uid == 1)
                {
                    _gameService.uid = playerData.uid;
                    _gameService.oid = playerData.oid;
                    self = view;
                }

                _playerService.AddPlayer(view.entity, playerData);
            }

            // 相机跟随
            RTSCamera.instance.targetFollow = (self as BaseGameView)?.gameObject.transform;
        }

        public override void Update()
        {
            if (!isRunning) return;

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
                if (player != null)
                    _inputService.ExecuteInputData(player.entity, input);
            }
        }

        private void SendInputs(int tick)
        {
            var frame = ObjectPool<FrameData>.Instance.Allocate();
            frame.tick = tick;
            frame.inputDatas = new[] {_inputService.inputData};
            _networkService.SendFrameData(frame);
        }

        #region receive message

        public void OnReceiveGameStart(GameStartMessage message)
        {
            InitializeSimulator(message);
        }

        public void OnReceiveFrame(FrameData frame)
        {
            _frameBuffer.PushSFrame(frame);
        }

        public void OnReceiveFrames(FrameData[] frames)
        {
            for (var i = 0; i < frames.Length; i++)
                _frameBuffer.PushSFrame(frames[i]);
        }

        #endregion
    }
}
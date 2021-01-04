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

        public Simulator simulator { get; private set; }

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

        #endregion data

        public void Start(GameStart data)
        {
            Logger.Debug("开始游戏");
            gameStart = data;
            tickDeltaTime = 1000 / Define.FRAME_RATE;
            simulator = new Simulator();
            simulator.Start(data);
        }

        public override void Update()
        {
            if (simulator == null || !simulator.isRunning) return;

            simulator.Update();

            tickSinceStart = (int)((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);
            while (simulator.tick < tickSinceStart)
            {
                simulator.Step();
            }

            while (inputTick <= inputPredictTick)
            {
                SendInputs(inputTick++);
            }
        }

        private void SendInputs(int tick)
        {
            var frame = new Frame()
            {
                tick = tick,
                playerInputs = new PlayerInput[Define.MAX_PLAYER_COUNT],
            };
            frame.playerInputs[0] = _inputService.curentInput;
            _networkService.SendFrame(frame);
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/31 11:55:03
 * @Description:
 */

namespace Lockstep.Logic
{
    public class SimulatorService : BaseService, ISimulatorService
    {
        #region simulator

        public Simulator simulator { get; private set; }

        /// <summary>
        /// 帧记录
        /// </summary>
        /// <value></value>
        public int tickSinceStart { get; private set; }

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

        public GameStartData gameStartData { get; private set; }

        #endregion data

        public void Start(GameStartData data)
        {
            gameStartData = data;
            tickDeltaTime = 1000 / Define.FRAME_RATE;
            simulator = new Simulator(LauncherClient.Instance.serviceContainer);
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
        }
    }
}
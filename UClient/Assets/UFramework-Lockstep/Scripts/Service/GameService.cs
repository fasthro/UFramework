/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:32:45
 * @Description: 帧同步模拟器服务
 */

using UFramework.Core;
using LSC;
using UnityEngine;
using UFramework.Network;
using PBBS;

namespace UFramework.Lockstep
{
    public class GameService : BaseService, IGameService
    {
        /// <summary>
        ///
        /// </summary>
        /// <value></value>
        public bool isRunning { get; private set; }

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

        /// <summary>
        /// 模拟器
        /// </summary>
        /// <value></value>
        public Simulator simulator { get; private set; }

        private ManagerService _managerService;

        public GameManager gameManager => _gameManager;
        public InputManager inputManager => _inputManager;
        public FrameManager frameManager => _frameManager;
        public ViewManager viewManager => _viewManager;

        private GameManager _gameManager;
        private InputManager _inputManager;
        private FrameManager _frameManager;
        private ViewManager _viewManager;


        public void Launch()
        {
            _managerService = Service.Instance.GetService<ManagerService>();

            _gameManager = RegisterManager<GameManager>();
            _inputManager = RegisterManager<InputManager>();
            _frameManager = RegisterManager<FrameManager>();
            _viewManager = RegisterManager<ViewManager>();

            startTime = LSTime.realtimeSinceStartupMS;
        }

        protected override void OnInitialize()
        {
            Messenger.AddListener<int, SocketPack>(GlobalEvent.NET_RECEIVED, OnNetReceived);

            tickDeltaTime = 1000 / LSDefine.FRAME_RATE;
            simulator = new Simulator(this);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (!isRunning) return;

            simulator.Update(tickDeltaTime);

            tickSinceStart = (int)((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);
            while (simulator.tick < tickSinceStart)
            {
                simulator.Step();
            }
        }

        private T RegisterManager<T>() where T : BaseManager, new()
        {
            var obj = new T();
            _managerService.RegisterManager(obj);
            return obj;
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case LSNetCmd.START:
                    var s2c = StartSimulate_S2C.Parser.ParseFrom(pack.rawData);
                    simulator.StartSimulate();
                    break;
            }
        }
    }
}
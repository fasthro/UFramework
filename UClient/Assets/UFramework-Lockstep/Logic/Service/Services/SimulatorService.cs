// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/31 11:55:03
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Lockstep.Logic.Simulator;
using Lockstep.Message;
using UFramework;
using UFramework.Core;
using UFramework.Network;
using UnityEngine;

namespace Lockstep.Logic
{
    public class SimulatorService : BaseGameBehaviour, ISimulatorService
    {
        #region simulator

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
        /// ping
        /// </summary>
        public SimulatorPing ping { get; private set; }

        #endregion simulator

        private FrameBuffer _frameBuffer;
        private RollbackInfo _rollbackInfo;

        public override void Initialize()
        {
            Messenger.AddListener<int, SocketPack>(GlobalEvent.NET_RECEIVED, OnNetReceived);
        }

        private void InitializeSimulator(GameStartMessage message)
        {
            isRunning = true;
            tickDeltaTime = 1000 / LSDefine.FRAME_RATE;
            _frameBuffer = new FrameBuffer();
            _rollbackInfo = new RollbackInfo();

            ping = new SimulatorPing();

            (_viewService as IGameRuntime)?.InitGame(message);
            (_uiService as IGameRuntime)?.InitGame(message);
            (_virtualJoyService as IGameRuntime)?.InitGame(message);
            (_inputService as IGameRuntime)?.InitGame(message);
            (_cameraService as IGameRuntime)?.InitGame(message);
        }

        public override void Update()
        {
            if (!isRunning) return;

            ping.Update();

            tickSinceStart = (int) ((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);

            // 回滚
            if (_rollbackInfo != null)
            {
                tick = _rollbackInfo.tick;
                _frameBuffer.Rollback(tick);
            }

            // 追帧
            while (tick < _frameBuffer.sTick)
            {
                var frame = _frameBuffer.GetFrame(tick + 1);
                if (frame != null)
                    ProcessFrame(frame);
            }

            // 客户端帧
            while (tick < tickSinceStart)
            {
                tick++;

                var frameData = ObjectPool<FrameData>.Instance.Allocate();
                frameData.tick = tick;
                if (_inputService.inputData != null)
                {
                    frameData.inputDatas = new[] {_inputService.inputData};

                    var message = new Frame_C2S() {Frame = frameData.ToLSMFrame()};
                    NetworkProxy.Send(NetworkCmd.PLAYER_INPUT, message);

                    _frameBuffer.PushCFrame(frameData);
                }
                else
                {
                    frameData.inputDatas = new InputData[0];
                    _frameBuffer.PushCFrame(frameData);
                }
            }

            // 执行帧
            while (_frameBuffer.rTick < tick)
            {
                var frame = _frameBuffer.GetFrame(_frameBuffer.rTick + 1);
                if (frame != null)
                {
                    ProcessFrame(frame);
                }
            }
        }

        private void ProcessFrame(FrameData frame)
        {
            foreach (var input in frame.inputDatas)
            {
                var player = _playerService.GetPlayer(input.oid);
                player?.entity.ReplaceCMovement(input.movementDir, false);
            }

            frame.Recycle();
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case NetworkCmd.PING:
                    var pm = Ping_S2C.Parser.ParseFrom(pack.rawData).ToPingMessage();
                    ping.AddValue(LSTime.realtimeSinceStartupMS - pm.sendTimestamp);
                    break;

                case NetworkCmd.START:
                    InitializeSimulator(GameStart_S2C.Parser.ParseFrom(pack.rawData).ToGameStartMessage());
                    break;

                case NetworkCmd.PUSH_FRAME:
                    _rollbackInfo = _frameBuffer.PushSFrame(Frame_S2C.Parser.ParseFrom(pack.rawData).Frame.ToFrameData());
                    break;
            }
        }
    }
}
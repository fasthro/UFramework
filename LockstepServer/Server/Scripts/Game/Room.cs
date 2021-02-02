// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Google.Protobuf;
using Lockstep;
using System;
using System.Collections.Generic;
using System.Linq;
using Lockstep.Message;
using UFramework;

namespace GameServer
{
    public class Room : BaseGameBehaviour
    {
        #region simulate

        /// <summary>
        /// </summary>
        /// <value></value>
        public bool isRunning { get; private set; }

        /// <summary>
        /// 当前帧
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

        #endregion simulate

        #region room

        public bool isFull => _players.Count < GameDefine.ROOM_PLAYER_MAX_COUNT;
        public bool isAllReady => _players.All(player => player.isReadyGame);
        public int roomId { get; }

        private string _secretKey;
        private Dictionary<long, int> u2oDict = new Dictionary<long, int>();
        private List<Player> _players = new List<Player>();
        private InputData[] _inputDatas;

        #endregion

        public Room(int roomId) : base()
        {
            this.roomId = roomId;
            _secretKey = Crypt.Base64Encode(Crypt.RandomKey());
            _players.Clear();
            isRunning = false;
        }

        public void EnterRoom(Player player)
        {
            var oid = _players.Count;
            if (!u2oDict.ContainsKey(player.uid))
            {
                u2oDict.Add(player.uid, _players.Count);
                _players.Add(player);
            }
            else oid = u2oDict[player.uid];

            player.EnterRoom(roomId, oid);
            LogHelper.Info($"Player [{player.uid}] Enter Room [{roomId}] , Room Player Count: {_players.Count}");
        }

        public void LeaveRoom(long uid)
        {
            foreach (var player in _players.Where(player => player.uid == uid))
            {
                _players.Remove(player);
                LogHelper.Info($"Player [{uid}] Leave Room [{roomId}] , Room Player Count: {_players.Count}");
                break;
            }
        }

        public void StartGame()
        {
            LogHelper.Info($"Room [{roomId}] Start Game.");

            _inputDatas = new InputData[_players.Count];

            tickDeltaTime = 1000 / Define.FRAME_RATE;
            startTime = LSTime.realtimeSinceStartupMS;
            isRunning = true;

            Send_StartGame();
        }

        public override void Update()
        {
            if (!isRunning)
                return;

            tickSinceStart = (int) ((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);
            while (tick < tickSinceStart)
            {
                tick++;
                Send_Frame();
            }
        }

        public void OnReceiveFrame(Player player, FrameData frameData)
        {
            if (frameData.tick < tick)
                return;

            _inputDatas[player.oid] = frameData.inputDatas[0];
        }

        private void Send_StartGame()
        {
            var s2c = new GameStart_S2C {Seed = new Random().Next()};
            s2c.Players.Clear();
            for (var i = 0; i < _players.Count; i++)
            {
                var player = _players[i];
                s2c.Players.Add(new LSMPlayer() {Uid = player.uid, Oid = i, Name = player.username});
            }

            SendMessage(NetwokCmd.START, s2c);
        }

        private void Send_Frame()
        {
            var frame = new LSMFrame() {Tick = tick};
            for (var i = 0; i < _inputDatas.Length; i++)
            {
                _inputDatas[i] ??= ObjectPool<InputData>.Instance.Allocate();
                frame.Inputs.Add(_inputDatas[i].ToLSMInputData());
            }

            var s2c = new Frame_S2C {Frame = frame};
            SendMessage(NetwokCmd.PUSH_FRAME, s2c);

            for (var i = 0; i < _inputDatas.Length; i++)
                _inputDatas[i] = null;
        }

        private void SendMessage(int cmd, IMessage message)
        {
            foreach (var player in _players)
                player?.session.PushCSharp(cmd, message);
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/23 11:47:10
 * @Description:
 */

using Google.Protobuf;
using LSC;
using PBBS;

namespace LockstepServer.Src
{
    public class Room : BaseGameBehaviour, IRoom
    {
        #region interface

        public int roomId => _roomId;

        public string secretKey => _secretKey;

        public int playerCount => _playerCount;

        #endregion interface

        #region private

        private int _roomId;
        private string _secretKey;
        private int _playerCount;

        private Player[] _players;

        #endregion private

        #region simulate

        public Simulator simulator { get; private set; }

        /// <summary>
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

        #endregion simulate

        public Room(int id) : base()
        {
            _roomId = id;
            _secretKey = Crypt.Base64Encode(Crypt.RandomKey());
            _players = new Player[GameDefine.ROOM_PLAYER_MAX_COUNT];
            _playerCount = 0;
            isRunning = false;
        }

        public bool isAllReady
        {
            get
            {
                bool ready = true;
                foreach (var player in _players)
                {
                    if (!player.isReady)
                    {
                        ready = false;
                        break;
                    }
                }
                return ready;
            }
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="player"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public int Enter(Player player, string secretKey)
        {
            if (_playerCount >= GameDefine.ROOM_PLAYER_MAX_COUNT)
                return -1;

            if (!this.secretKey.Equals(secretKey))
                return -2;

            _players[_playerCount] = player;
            _playerCount++;
            LogHelper.Info($"玩家[{player.uid}]进入房间[{roomId}], 当前房间玩家数量{playerCount}个");
            return ResultCode.SUCCEED;
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="uid"></param>
        public void Remove(long uid)
        {
            for (int i = 0; i < GameDefine.ROOM_PLAYER_MAX_COUNT; i++)
            {
                if (_players[i].uid == uid)
                {
                    _players[i] = null;
                    _playerCount--;
                    break;
                }
            }
            LogHelper.Info($"玩家[{uid}]离开房间[{roomId}], 当前房间玩家数量{playerCount}个");
        }

        /// <summary>
        /// 玩家准备好
        /// </summary>
        /// <param name="player"></param>
        public void Ready(long uid)
        {
            foreach (var player in _players)
            {
                if (player.uid == uid)
                {
                    player.isReady = true;
                    break;
                }
            }
            LogHelper.Info($"玩家[{uid}]已准备");
        }

        /// <summary>
        /// 开始模拟
        /// </summary>
        public void StartSimulate()
        {
            tickDeltaTime = 1000 / LSDefine.FRAME_RATE;
            startTime = LSTime.realtimeSinceStartupMS;

            simulator = new Simulator();

            PushMessage_StartSimulate();
        }

        public void PushMessage(int cmd, IMessage message)
        {
            foreach (var player in _players)
                player?.session.PushCSharp(cmd, message);
        }

        public override void Update()
        {
            if (!isRunning)
                return;

            if (simulator == null)
                return;

            tickSinceStart = (int)((LSTime.realtimeSinceStartupMS - startTime) / tickDeltaTime);
            while (simulator.tick < tickSinceStart)
            {
                simulator.Update();
            }
        }

        private void PushMessage_StartSimulate()
        {
            var s2c = new StartSimulate_S2C();
            s2c.Seed = simulator.seed;
            s2c.Users.Clear();
            foreach (var player in _players)
            {
                var info = new PBBSCommon.User();
                info.UserId = player.uid;
                info.UserName = "Name " + player.uid;
                s2c.Users.Add(info);
            }
            PushMessage(NetwokCmd.START, s2c);
        }
    }
}
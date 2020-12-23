/*
 * @Author: fasthro
 * @Date: 2020/12/23 11:47:10
 * @Description:
 */

using PBBattleServer;

namespace LockstepServer.Src
{
    public class Room : BaseBehaviour, IRoom
    {
        #region interface

        public int roomId => _roomId;

        public string secretKey => _secretKey;

        public RoomStatus status => _status;

        public int count => _count;

        #endregion interface

        #region private

        private int _roomId;
        private string _secretKey;
        private int _count;
        private RoomStatus _status;

        private Player[] _players;

        #endregion private

        public Room(int id)
        {
            _roomId = id;
            _secretKey = Crypt.Base64Encode(Crypt.RandomKey());

            _status = RoomStatus.Underfill;
            _count = 0;

            _players = new Player[GameDefine.ROOM_PLAYER_MAX_COUNT];
        }

        public bool Enter(Player player, string secretKey)
        {
            if (_status != RoomStatus.Underfill)
                return false;

            if (!this.secretKey.Equals(secretKey))
                return false;

            _players[_count] = player;
            _count++;
            if (_count >= GameDefine.ROOM_PLAYER_MAX_COUNT)
            {
                _status = RoomStatus.Ready;
            }
            LogHelper.Info($"玩家[{player.uid}]进入房间[{roomId}], 当前房间玩家数量{count}个");
            return true;
        }

        public void Start()
        {
            BattleStart_S2C s2c = new BattleStart_S2C();
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].session.Send(950, -1, s2c);
            }
            LogHelper.Info($"房间[{roomId}]玩家已满，开始战斗");
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (status != RoomStatus.Ready) return;

            var s2c = new Frame_S2C();
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].session.Send(951, -1, s2c);
            }
        }
    }
}
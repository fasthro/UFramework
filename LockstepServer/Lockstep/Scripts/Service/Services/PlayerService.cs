// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Lockstep
{
    public class PlayerService : BaseService, IPlayerService
    {
        #region public

        public Player self => _self;

        #endregion

        #region private

        private Dictionary<long, Player> _playerDict = new Dictionary<long, Player>();
        private Dictionary<int, long> _o2uDict = new Dictionary<int, long>();

        private Player _self;

        #endregion

        public override void Initialize()
        {
            _playerDict.Clear();
            _o2uDict.Clear();
        }

        public void AddPlayer(GameEntity entity, PlayerData playerData)
        {
            if (!_playerDict.ContainsKey(playerData.uid))
            {
                var player = Player.Allocate(entity, playerData);

                _playerDict.Add(playerData.uid, player);
                _o2uDict.Add(playerData.oid, playerData.uid);

                if (playerData.uid == _gameService.uid)
                {
                    _self = player;
                }
            }
        }

        public Player GetPlayer(int oid)
        {
            return _o2uDict.ContainsKey(oid) ? _playerDict[_o2uDict[oid]] : null;
        }

        public Player GetPlayer(long uid)
        {
            return _playerDict.ContainsKey(uid) ? _playerDict[uid] : null;
        }
    }
}
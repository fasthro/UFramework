/*
 * @Author: fasthro
 * @Date: 2020/12/23 14:06:54
 * @Description:
 */

using System.Collections.Generic;

namespace LockstepServer.Src
{
    public class PlayerService : BaseService, IPlayerService
    {
        public bool ExistPlayer(long uid)
        {
            return _playerDict.ContainsKey(uid);
        }

        public Player GetPlayer(long uid)
        {
            if (_playerDict.ContainsKey(uid))
                return _playerDict[uid];
            return null;
        }

        public Player GetPlayer(int sessionId)
        {
            if (_sessionDict.ContainsKey(sessionId))
            {
                var uid = _sessionDict[sessionId];
                if (_playerDict.ContainsKey(uid))
                    return _playerDict[uid];
            }
            return null;
        }

        public void AddPlayer(Player player)
        {
            if (!_playerDict.ContainsKey(player.uid))
            {
                _playerDict.Add(player.uid, player);
                _sessionDict.Add(player.session.sessionId, player.uid);
            }
        }

        public void RemovePlayer(long uid)
        {
            if (_playerDict.ContainsKey(uid))
            {
                var player = _playerDict[uid];
                _sessionDict.Remove(player.session.sessionId);
                _playerDict.Remove(uid);
            }
        }

        public override void Initialize()
        {
            _playerDict.Clear();
            _sessionDict.Clear();
        }

        private Dictionary<long, Player> _playerDict = new Dictionary<long, Player>();
        private Dictionary<int, long> _sessionDict = new Dictionary<int, long>();
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/23 14:06:54
 * @Description:
 */

using System.Collections.Generic;
using UFramework;

namespace GameServer
{
    public class PlayerManager : BaseManager, IPlayerManager
    {
        private Dictionary<long, Player> _playerDict = new Dictionary<long, Player>();
        private Dictionary<int, long> _sessionDict = new Dictionary<int, long>();

        public bool ExistPlayer(long uid)
        {
            return _playerDict.ContainsKey(uid);
        }

        public Player GetPlayer(long uid)
        {
            return _playerDict.ContainsKey(uid) ? _playerDict[uid] : null;
        }

        public Player GetPlayer(int sessionId)
        {
            return _sessionDict.ContainsKey(sessionId) ? _playerDict[_sessionDict[sessionId]] : null;
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
    }
}
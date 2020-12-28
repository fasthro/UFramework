/*
 * @Author: fasthro
 * @Date: 2020-12-16 13:55:30
 * @Description:
 */

using System.Collections.Generic;

namespace UFramework.Lockstep
{
    public class ViewManager : BaseManager
    {
        private Dictionary<long, IPlayerView> playerDict = new Dictionary<long, IPlayerView>();

        public void AddPlayer(long id, IPlayerView vo)
        {
            if (!playerDict.ContainsKey(id))
            {
                playerDict.Add(id, vo);
            }
        }

        public IPlayerView GetPlayer(long id)
        {
            if (playerDict.ContainsKey(id))
            {
                return playerDict[id];
            }
            return null;
        }

        public void RemovePlayer(long id)
        {
            if (playerDict.ContainsKey(id))
            {
                playerDict.Remove(id);
            }
        }
    }
}
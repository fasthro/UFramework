/*
 * @Author: fasthro
 * @Date: 2020/12/23 14:03:25
 * @Description:
 */

namespace LockstepServer.Src
{
    public class Player : IPlayer
    {
        #region private

        private long _uid;
        private ISession _session;

        #endregion private

        #region interface

        public long uid => _uid;
        public ISession session => _session;

        #endregion interface

        #region data

        public bool isReady { get; set; }

        #endregion data

        public Player(long uid, ISession session)
        {
            _uid = uid;
            _session = session;
        }
    }
}
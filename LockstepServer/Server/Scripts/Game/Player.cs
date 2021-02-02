// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep;
using UFramework;

namespace GameServer
{
    public class Player : BaseGameBehaviour
    {
        public long uid { get; }
        public int oid { get; private set; }
        public int sid => session.sessionId;
        public string username { get; }
        public ISession session { get; }

        public bool insideRoom => roomId != -1;
        public int roomId { get; private set; }
        public bool isReadyGame { get; private set; }

        public Player(UserInfo info, ISession session)
        {
            this.uid = info.uid;
            this.username = info.username ?? "";
            this.session = session;
            this.roomId = -1;
        }

        public void EnterRoom(int roomId, int oid)
        {
            this.roomId = roomId;
            this.oid = oid;
        }

        public void LeaveRoom()
        {
            roomId = -1;
            oid = -1;
            isReadyGame = false;
        }

        public void ReadyGame()
        {
            isReadyGame = true;
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:24:40
 * @Description:
 */

namespace LockstepServer.Src
{
    public class RoomManager : BaseManager
    {
        private static int ROOMID = 1;
        public Room room { get; private set; }

        public bool EnterRoom(Player player, string secretKey)
        {
            return room.Enter(player, secretKey); ;
        }

        protected override void OnInitialize()
        {
            room = new Room(1);
        }

        protected override void OnUpdate(float deltaTime)
        {
            room.DoUpdate(deltaTime);
        }
    }
}
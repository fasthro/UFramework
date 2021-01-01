/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:24:40
 * @Description:
 */

namespace LockstepServer.Src
{
    public class RoomService : BaseService, IRoomService
    {
        public Room room => _room;

        public override void Initialize()
        {
            _room = new Room(1);
        }

        public override void Update()
        {
            room.Update();
        }

        private static int ROOMID = 1;
        private Room _room;
    }
}
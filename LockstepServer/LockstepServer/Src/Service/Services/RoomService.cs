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
            CreateNewRoom();
        }

        public void CreateNewRoom()
        {
            LogHelper.Debug("创建新房间");
            _room = new Room(1);
        }

        public override void Update()
        {
            room.Update();
        }

        private Room _room;
    }
}
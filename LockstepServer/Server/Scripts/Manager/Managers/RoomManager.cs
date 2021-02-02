/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:24:40
 * @Description:
 */

using System.Collections.Generic;
using System.Linq;
using Lockstep;
using Lockstep.Message;
using UFramework;

namespace GameServer
{
    public class RoomManager : BaseManager, IRoomManager
    {
        /// <summary>
        /// 自动创建房间数
        /// </summary>
        private const int DEFAULT_CREAT_COUNT = 10;

        private List<Room> _rooms;

        public override void Initialize()
        {
            _rooms = new List<Room>();

            for (var i = 0; i < DEFAULT_CREAT_COUNT; i++)
                _rooms.Add(new Room(i));
        }

        public override void Update()
        {
            foreach (var room in _rooms.Where(room => room.isRunning))
                room.Update();
        }

        public void EnterRoom(Player player)
        {
            if (player.roomId == -1)
            {
                foreach (var room in _rooms.Where(room => !room.isFull))
                {
                    room.EnterRoom(player);
                    break;
                }

                var newRoom = new Room(_rooms.Count);
                _rooms.Add(newRoom);
                newRoom.EnterRoom(player);
            }
            else _rooms[player.roomId].EnterRoom(player);
        }

        public void LeaveRoom(long uid, int roomId)
        {
            _rooms[roomId].LeaveRoom(uid);
        }

        public void RoomTryStartGame(int roomId)
        {
            var room = _rooms[roomId];
            if (room.isAllReady)
            {
                room.StartGame();
            }
        }

        public void OnReceiveFrame(Player player, FrameData frameData)
        {
            if (player.insideRoom)
                _rooms[player.roomId].OnReceiveFrame(player, frameData);
        }
    }
}
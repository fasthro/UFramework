/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:49:52
 * @Description:
 */

using Lockstep;
using UFramework;

namespace GameServer
{
    public interface IRoomManager : IManager
    {
        void EnterRoom(Player player);
        void RoomTryStartGame(int roomId);
        void LeaveRoom(long uid, int roomId);
        void OnReceiveFrame(Player player, FrameData frameData);
    }
}
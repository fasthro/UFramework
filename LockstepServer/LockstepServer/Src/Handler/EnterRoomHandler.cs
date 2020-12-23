/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:23:57
 * @Description: 进入房间
 */

using LiteNetLib;
using PBBattleServer;

namespace LockstepServer.Src
{
    public class EnterRoomHandler : IHandler
    {
        public int cmd => 902;

        public void OnMessage(NetPeer peer, int session, byte[] bytes)
        {
            var playerManager = Service.Instance.GetManager<PlayerManager>();
            var roomManager = Service.Instance.GetManager<RoomManager>();

            #region rec

            EnterRoom_C2S c2s = EnterRoom_C2S.Parser.ParseFrom(bytes);
            LogHelper.Info($"客户端进入房间请求[{c2s.RoomSecretKey}]");

            bool result = false;
            var player = playerManager.GetPlayer(peer.Id);
            if (player != null)
            {
                result = roomManager.EnterRoom(player, c2s.RoomSecretKey);
            }

            #endregion rec

            #region send

            var s2c = new EnterRoom_S2C();
            s2c.ResultCode = result ? ResultCode.SUCCEED : ResultCode.FAILED;
            Service.Instance.GetManager<NetManager>().Send(peer, cmd, session, s2c);

            #endregion send

            if (result)
            {
                if (roomManager.room.status == RoomStatus.Ready)
                {
                    roomManager.room.Start();
                }
            }
        }
    }
}
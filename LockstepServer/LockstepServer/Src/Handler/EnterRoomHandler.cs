/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:23:57
 * @Description: 进入房间
 */

using LiteNetLib;
using PBBattleServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    public class EnterRoomHandler : IHandler
    {
        public int cmd => 902;

        public void OnMessage(NetPeer peer, int session, byte[] bytes)
        {
            var msg = new EnterRoom_S2C();
            msg.ResultCode = ResultCode.SUCCEED;

            Service.Instance.GetManager<NetManager>().Send(peer, cmd, session, msg);
        }
    }
}
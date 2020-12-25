/*
 * @Author: fasthro
 * @Date: 2020/12/24 15:51:56
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;
using PBBS;

namespace LockstepServer.Src
{
    public class ReadyHandler : BaseHandler
    {
        public override int cmd => 903;
        public RoomManager roomManager => Service.Instance.GetManager<RoomManager>();

        protected override void OnMessage(byte[] bytes)
        {
            Ready_C2S c2c = Ready_C2S.Parser.ParseFrom(bytes);
            roomManager.room.Ready(c2c.Uid);
        }

        protected override bool OnResponse()
        {
            CreateResponseMessage<Ready_S2C>();
            return true;
        }

        protected override void OnAction()
        {
            if (roomManager.room.isAllReady)
            {
                roomManager.room.StartSimulate();
            }
        }
    }
}
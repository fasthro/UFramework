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
    public class ReadyHandler : BaseGameHandler
    {
        public override int cmd => 903;

        protected override void OnMessage(byte[] bytes)
        {
            Ready_C2S c2s = Ready_C2S.Parser.ParseFrom(bytes);
            _roomService.room.Ready(c2s.Uid);
        }

        protected override bool OnResponse()
        {
            CreateResponseMessage<Ready_S2C>();
            return true;
        }

        protected override void OnAction()
        {
            if (_roomService.room.isAllReady)
            {
                _roomService.room.StartSimulate();
            }
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/31 15:24:51
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    public class ConnectHandler : BaseGameHandler
    {
        public override int cmd => NetwokCmd.CLIENT_CONNECT;

        protected override void OnMessage(byte[] bytes)
        {
        }

        protected override bool OnResponse()
        {
            return false;
        }
    }
}
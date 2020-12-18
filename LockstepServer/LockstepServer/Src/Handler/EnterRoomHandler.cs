/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:23:57
 * @Description:
 */

using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    public class EnterRoomHandler : IUdpSocketHandler
    {
        public void OnMessage(NetPeer peer, byte[] bytes)
        {
        }
    }
}
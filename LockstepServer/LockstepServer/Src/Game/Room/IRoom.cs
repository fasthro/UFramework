/*
 * @Author: fasthro
 * @Date: 2020/12/23 11:49:00
 * @Description:
 */

using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    internal interface IRoom
    {
        int roomId { get; }
        string secretKey { get; }
        int playerCount { get; }
        bool isEmpty { get; }

        void PushMessage(int cmd, IMessage message);
    }
}
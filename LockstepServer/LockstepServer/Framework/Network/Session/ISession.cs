/*
 * @Author: fasthro
 * @Date: 2020/12/23 14:21:16
 * @Description:
 */

using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public interface ISession
    {
        int sessionId { get; }

        void Send(int cmd, int session, IMessage message);

        void Kick();
    }
}
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

        void Kick();

        void Send(int cmd, int session, IMessage message);

        void SendLua(int cmd, int session, IMessage message);

        void SendCSharp(int cmd, int session, IMessage message);

        void Push(int cmd, IMessage message);

        void PushLua(int cmd, IMessage message);

        public void PushCSharp(int cmd, IMessage message);
    }
}
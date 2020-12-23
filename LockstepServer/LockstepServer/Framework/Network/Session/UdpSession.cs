/*
 * @Author: fasthro
 * @Date: 2020/12/23 14:19:59
 * @Description:
 */

using Google.Protobuf;
using LiteNetLib;
using System;

namespace LockstepServer
{
    public class UdpSession : ISession
    {
        private NetPeer _connection;

        public UdpSession(NetPeer peer)
        {
            this._connection = peer;
        }

        public int sessionId => _connection.Id;

        public void Kick()
        {
            throw new NotImplementedException();
        }

        public void Send(int cmd, int session, IMessage message)
        {
            Service.Instance.GetManager<NetManager>().Send(_connection, cmd, session, message);
        }
    }
}
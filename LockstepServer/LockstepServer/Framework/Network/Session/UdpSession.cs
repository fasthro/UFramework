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
    public class UdpSession : BaseBehaviour, ISession
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
            _networkService.Send(_connection, cmd, session, NetworkProcessLayer.All, message);
        }

        public void SendLua(int cmd, int session, IMessage message)
        {
            _networkService.Send(_connection, cmd, session, NetworkProcessLayer.Lua, message);
        }

        public void SendCSharp(int cmd, int session, IMessage message)
        {
            _networkService.Send(_connection, cmd, session, NetworkProcessLayer.CSharp, message);
        }

        public void Push(int cmd, IMessage message)
        {
            _networkService.Send(_connection, cmd, 0, NetworkProcessLayer.All, message);
        }

        public void PushLua(int cmd, IMessage message)
        {
            _networkService.Send(_connection, cmd, 0, NetworkProcessLayer.Lua, message);
        }

        public void PushCSharp(int cmd, IMessage message)
        {
            _networkService.Send(_connection, cmd, 0, NetworkProcessLayer.CSharp, message);
        }
    }
}
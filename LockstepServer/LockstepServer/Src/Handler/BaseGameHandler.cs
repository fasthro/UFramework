/*
 * @Author: fasthro
 * @Date: 2020/12/24 12:03:19
 * @Description:
 */

using Google.Protobuf;
using LiteNetLib;

namespace LockstepServer.Src
{
    public abstract class BaseGameHandler : BaseGameBehaviour, IHandler
    {
        public NetPeer peer { get; set; }

        public abstract int cmd { get; }

        public int session { get; set; }
        public NetworkProcessLayer layer { get; set; }
        public IMessage responseMessage { get; set; }

        public void DoMessage(NetPeer peer, int session, NetworkProcessLayer layer, byte[] bytes)
        {
            this.peer = peer;
            this.session = session;
            this.layer = layer;

            OnMessage(bytes);
        }

        public bool DoResponse()
        {
            return OnResponse();
        }

        public void DoAction()
        {
            OnAction();
        }

        protected abstract void OnMessage(byte[] bytes);

        protected abstract bool OnResponse();

        protected T CreateResponseMessage<T>() where T : IMessage, new()
        {
            var message = new T();
            responseMessage = message;
            return message;
        }

        protected virtual void OnAction()
        {
        }
    }
}
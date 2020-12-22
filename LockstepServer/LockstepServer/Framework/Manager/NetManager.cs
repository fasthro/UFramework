/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:28:02
 * @Description: 网络管理
 */

using Google.Protobuf;
using LiteNetLib;
using LiteNetLib.Utils;

namespace LockstepServer
{
    internal class NetManager : BaseManager, IServerListener
    {
        /// <summary>
        /// 头长度
        /// byte[] [0-3]:cmd [4-7]:session
        /// </summary>
        private const int PACK_HEADER_SIZE = 8;

        private FixedByteArray _fixedReader;
        private byte[] _fixedBuff;

        private HandlerManager _handlerManager;

        #region base

        protected override void OnInitialize()
        {
            _fixedReader = new FixedByteArray(PACK_HEADER_SIZE);
            _fixedBuff = new byte[PACK_HEADER_SIZE];
        }

        #endregion base

        #region IServerListener

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            if (_handlerManager == null)
                _handlerManager = Service.Instance.GetManager<HandlerManager>();

            reader.GetUShort();

            reader.GetBytes(_fixedBuff, PACK_HEADER_SIZE);
            _fixedReader.Write(_fixedBuff);

            var cmd = _fixedReader.ReadInt32();
            var session = _fixedReader.ReadInt32();

            LogHelper.Debug($"收到客户端协议[{peer.EndPoint}] cmd:{cmd} session:{session}");

            var count = reader.AvailableBytes;
            byte[] bytes = new byte[count];
            reader.GetBytes(bytes, count);

            _handlerManager.OnReceive(peer, cmd, session, bytes);
        }

        public void OnPeerConnected(NetPeer peer)
        {
            LogHelper.Info("客户端连接成功 [" + peer.EndPoint + "]");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            LogHelper.Info("客户端连接断开 [" + peer.EndPoint + "]");
        }

        #endregion IServerListener

        #region send

        public void Send(NetPeer peer, int cmd, int session, IMessage message)
        {
            var data = message.ToByteArray();

            var _writer = new NetDataWriter();
            _writer.Reset();
            _writer.Put((ushort)(data.Length + PACK_HEADER_SIZE));
            _writer.Put(cmd);
            _writer.Put(session);
            _writer.Put(data);

            peer.Send(_writer, DeliveryMethod.ReliableOrdered);
        }

        #endregion send
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using Google.Protobuf;
using LiteNetLib;
using LiteNetLib.Utils;

namespace UFramework
{
    public class NetworkManager : BaseManager, INetworkManager, IServerListener
    {
        /// <summary>
        /// 头长度 byte[] [1-4]:cmd [5-8]:session [9-10]:layer
        /// </summary>
        private const int PACK_HEADER_SIZE = 10;

        private FixedByteArray _fixedReader;
        private FixedByteArray _fixedWriter;
        private byte[] _fixedBuff;

        #region base

        public override void Initialize()
        {
            _fixedReader = new FixedByteArray(PACK_HEADER_SIZE);
            _fixedWriter = new FixedByteArray(PACK_HEADER_SIZE);
            _fixedBuff = new byte[PACK_HEADER_SIZE];
        }

        #endregion base

        #region IServerListener

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            reader.GetUShort();

            reader.GetBytes(_fixedBuff, PACK_HEADER_SIZE);
            _fixedReader.Write(_fixedBuff);

            var cmd = _fixedReader.ReadInt32();
            var session = _fixedReader.ReadInt32();
            var layer = (NetworkProcessLayer) _fixedReader.ReadInt16();

            LogHelper.Debug($"收到客户端协议[{peer.EndPoint}] cmd:{cmd} session:{session} layer:{layer}");

            var count = reader.AvailableBytes;
            var bytes = new byte[count];
            reader.GetBytes(bytes, count);

            _handlerManager.OnReceive(peer, cmd, session, layer, bytes);
        }

        public void OnPeerConnected(NetPeer peer)
        {
            // LogHelper.Info("客户端连接成功 [" + peer.EndPoint + "]");
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            // LogHelper.Info("客户端连接断开 [" + peer.EndPoint + "]");

            _handlerManager.OnReceive(peer, GameConst.NETWORK_CMD_CLIENT_DISCONNECT, 0, 0, null);
        }

        #endregion IServerListener

        #region send

        public void Send(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, IMessage message)
        {
            _fixedWriter.Clear();
            _fixedWriter.Write(cmd);
            _fixedWriter.Write(session);
            _fixedWriter.Write((short) layer);

            var data = message.ToByteArray();

            var _writer = new NetDataWriter();
            _writer.Reset();
            _writer.Put((ushort) (data.Length + PACK_HEADER_SIZE));
            _writer.Put(_fixedWriter.buffer);
            _writer.Put(data);
            peer.Send(_writer, DeliveryMethod.ReliableOrdered);
        }

        #endregion send
    }
}
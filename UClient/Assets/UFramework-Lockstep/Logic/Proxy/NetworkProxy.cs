using Google.Protobuf;
using UFramework;
using UFramework.Network;

namespace Lockstep.Logic
{
    public class NetworkProxy
    {
        static NetworkManager _network;
        public static NetworkManager network
        {
            get
            {
                if (_network == null)
                {
                    _network = AppLauncher.Main.managerContainer.GetManager<NetworkManager>();
                }
                return _network;
            }
        }

        public static void Send(int cmd, IMessage message)
        {
            var pack = SocketPack.AllocateWriter(PackType.SizeHeaderBinary, cmd, ProcessLayer.CSharp);
            pack.WriteBuffer(message.ToByteArray());
            network.Send(2, pack);
        }
    }
}
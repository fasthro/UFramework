// --------------------------------------------------------------------------------

using Google.Protobuf;

namespace UFramework
{
    public interface IHandler : IUdpSocketHandler
    {
        int cmd { get; }
        int session { get; set; }
        NetworkProcessLayer layer { get; set; }
        IMessage responseMessage { get; set; }

        bool DoResponse();

        void DoAction();
    }
}
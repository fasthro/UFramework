// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/31 12:07:37
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep.Message;
using UFramework;
using UFramework.Core;
using UFramework.Network;

namespace Lockstep.Logic
{
    public class NetworkService : BaseGameBehaviour, INetworkService
    {
        public override void Initialize()
        {
            Messenger.AddListener<int, SocketPack>(GlobalEvent.NET_RECEIVED, OnNetReceived);
        }

        public void SendPing()
        {
            var message = new Ping_C2S()
            {
                Oid = _gameService.oid,
                SendTimestamp = LSTime.realtimeSinceStartupMS
            };
            NetworkProxy.Send(NetworkCmd.PING, message);
        }

        public void SendInput(FrameData frameData)
        {
            var message = new Frame_C2S() {Frame = frameData.ToLSMFrame()};
            // frameData.Recycle();
            NetworkProxy.Send(NetworkCmd.PLAYER_INPUT, message);
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case NetworkCmd.PING:
                    _simulatorService.OnReceivePing(Ping_S2C.Parser.ParseFrom(pack.rawData).ToPingMessage());
                    break;

                case NetworkCmd.START:
                    _simulatorService.OnReceiveGameStart(GameStart_S2C.Parser.ParseFrom(pack.rawData).ToGameStartMessage());
                    break;

                case NetworkCmd.PUSH_FRAME:
                    _simulatorService.OnReceiveFrame(Frame_S2C.Parser.ParseFrom(pack.rawData).Frame.ToFrameData());
                    break;
            }
        }
    }
}
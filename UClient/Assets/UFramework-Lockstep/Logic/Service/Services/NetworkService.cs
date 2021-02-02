/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:07:37
 * @Description:
 */

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

        public void SendFrameData(FrameData frameData)
        {
            var message = new Frame_C2S() {Frame = frameData.ToLSMFrame()};
            frameData.Recycle();
            NetworkProxy.Send(NetworkCmd.PLAYER_INPUT, message);
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case NetworkCmd.START:
                    OnReceiveStart(GameStart_S2C.Parser.ParseFrom(pack.rawData));
                    break;

                case NetworkCmd.PUSH_FRAME:
                    OnReceivePushFrame(Frame_S2C.Parser.ParseFrom(pack.rawData));
                    break;
            }
        }

        private void OnReceiveStart(GameStart_S2C s2c)
        {
            _simulatorService.OnReceiveGameStart(s2c.ToGameStartMessage());
        }

        private void OnReceivePushFrame(Frame_S2C s2c)
        {
            _simulatorService.OnReceiveFrame(s2c.Frame.ToFrameData());
        }
    }
}
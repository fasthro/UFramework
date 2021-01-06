/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:07:37
 * @Description:
 */
using PBBS;
using UFramework;
using UFramework.Core;
using UFramework.Network;
using Lockstep.MessageData;

namespace Lockstep.Logic
{
    public class NetworkService : BaseGameBehaviour, INetworkService
    {
        public override void Initialize()
        {
            Messenger.AddListener<int, SocketPack>(GlobalEvent.NET_RECEIVED, OnNetReceived);
        }

        public void SendFrame(Frame frame)
        {
            var msg = new Frame_C2S();
            msg.Frame = (PBBSCommon.Frame)frame.ToMessage();
            frame.Recycle();
            NetworkProxy.Send(NetworkCmd.PLAYER_INPUT, msg);
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case NetworkCmd.START:
                    OnReceiveStart(StartSimulate_S2C.Parser.ParseFrom(pack.rawData));
                    break;

                case NetworkCmd.PUSH_FRAME:
                    OnReceivePushFrame(Frame_S2C.Parser.ParseFrom(pack.rawData));
                    break;
            }
        }

        private void OnReceiveStart(StartSimulate_S2C s2c)
        {
            var userCount = s2c.Users.Count;

            var data = ObjectPool<GameStart>.Instance.Allocate();
            data.randSeed = s2c.Seed;
            data.users = new User[userCount];
            for (int i = 0; i < userCount; i++)
            {
                var user = ObjectPool<User>.Instance.Allocate();
                user.FromMessage(s2c.Users[i]);
                data.users[i] = user;
            }
            _simulatorService.Start(data);
        }

        private void OnReceivePushFrame(Frame_S2C s2c)
        {
            if (s2c.Frames.Count == 1)
            {
                var frame = ObjectPool<Frame>.Instance.Allocate();
                frame.FromMessage(s2c.Frames[0]);
                _simulatorService.OnReceiveFrame(frame);
            }
            else
            {
                var frameCount = s2c.Frames.Count;
                var frames = new Frame[frameCount];
                for (int i = 0; i < frameCount; i++)
                {
                    var frame = ObjectPool<Frame>.Instance.Allocate();
                    frame.FromMessage(s2c.Frames[i]);
                    frames[i] = frame;
                }
                _simulatorService.OnReceiveFrames(frames);
            }
        }
    }
}
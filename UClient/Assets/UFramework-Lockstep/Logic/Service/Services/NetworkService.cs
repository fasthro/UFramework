/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:07:37
 * @Description:
 */
using PBBS;
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

        public void SendFrame(Frame frame)
        {
            var msg = new Frame_C2S()
            {
                Frame = new PBBSCommon.Frame()
            };
            for (int i = 0; i < frame.playerInputs.Length; i++)
            {
                var input = frame.playerInputs[i];
                msg.Frame.Inputs.Add(new PBBSCommon.PlayerInput()
                {
                    Px = input.px,
                    Py = input.py,
                });
            }

            NetworkProxy.Send(NetworkCmd.PLAYER_INPUT, msg);
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case NetworkCmd.START:
                    var s2c = StartSimulate_S2C.Parser.ParseFrom(pack.rawData);
                    var userCount = s2c.Users.Count;
                    var data = new GameStart
                    {
                        randSeed = s2c.Seed,
                        users = new User[userCount]
                    };
                    for (int i = 0; i < userCount; i++)
                    {
                        var user = new User
                        {
                            id = s2c.Users[i].Id
                        };
                        data.users[i] = user;
                    }
                    _simulatorService.Start(data);
                    break;

                case NetworkCmd.PUSH_FRAME:

                    break;
            }
        }
    }
}
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
    public class NetworkService : BaseService, INetworkService
    {
        public override void Initialize()
        {
            Messenger.AddListener<int, SocketPack>(GlobalEvent.NET_RECEIVED, OnNetReceived);
        }

        private void OnNetReceived(int channelId, SocketPack pack)
        {
            switch (pack.cmd)
            {
                case NetworkCmd.START:
                    var s2c = StartSimulate_S2C.Parser.ParseFrom(pack.rawData);
                    var userCount = s2c.Users.Count;
                    var data = new GameStartData();
                    data.randSeed = s2c.Seed;
                    data.users = new UserData[userCount];
                    for (int i = 0; i < userCount; i++)
                    {
                        var user = new UserData();
                        user.userId = s2c.Users[i].UserId;
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
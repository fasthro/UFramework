// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/01 16:30
// * @Description:
// --------------------------------------------------------------------------------

using Lockstep.Message;

namespace Lockstep
{
    public static class MessageExtension
    {
        public static LSVector3 ToLSVector3(this LSMVector3 vec)
        {
            return new LSVector3(vec.X, vec.Y, vec.Z);
        }

        public static LSMVector3 ToLSMVector3(this LSVector3 vec)
        {
            return new LSMVector3() {X = (double) vec.x, Y = (double) vec.y, Z = (double) vec.z};
        }

        public static PlayerData ToPlayerData(this LSMPlayer player)
        {
            var data = ObjectPool<PlayerData>.Instance.Allocate();
            data.uid = player.Uid;
            data.oid = player.Oid;
            data.name = player.Name;
            return data;
        }

        public static LSMPlayer ToLSMPlayer(this PlayerData player)
        {
            var data = new LSMPlayer()
            {
                Uid = player.uid,
                Oid = player.oid,
                Name = player.name,
            };
            return data;
        }

        public static InputData ToInputData(this LSMInput input)
        {
            var data = ObjectPool<InputData>.Instance.Allocate();
            data.oid = input.Oid;
            data.movementDir = input.MovementDir.ToLSVector3();
            return data;
        }

        public static LSMInput ToLSMInputData(this InputData input)
        {
            var data = new LSMInput()
            {
                Oid = input.oid,
                MovementDir = input.movementDir.ToLSMVector3()
            };
            return data;
        }

        public static FrameData ToFrameData(this LSMFrame frame)
        {
            var data = ObjectPool<FrameData>.Instance.Allocate();
            data.tick = frame.Tick;
            data.inputDatas = new InputData[frame.Inputs.Count];
            int index = 0;
            foreach (var input in frame.Inputs)
            {
                data.inputDatas[index] = new InputData()
                {
                    movementDir = input.MovementDir.ToLSVector3()
                };
                index++;
            }

            return data;
        }

        public static LSMFrame ToLSMFrame(this FrameData frame)
        {
            var data = new LSMFrame();
            data.Tick = frame.tick;
            data.Inputs.Clear();
            foreach (var input in frame.inputDatas)
            {
                if (input != null)
                {
                    data.Inputs.Add(new LSMInput()
                    {
                        MovementDir = input.movementDir.ToLSMVector3()
                    });
                }
            }

            return data;
        }

        public static GameStartMessage ToGameStartMessage(this GameStart_S2C message)
        {
            var msg = new GameStartMessage();
            msg.randSeed = message.Seed;
            msg.playerDatas = new PlayerData[message.Players.Count];
            int index = 0;
            foreach (var player in message.Players)
            {
                msg.playerDatas[index] = player.ToPlayerData();
                index++;
            }

            return msg;
        }

        public static PingMessage ToPingMessage(this Ping_S2C message)
        {
            return new PingMessage {sendTimestamp = message.SendTimestamp};
        }
    }
}
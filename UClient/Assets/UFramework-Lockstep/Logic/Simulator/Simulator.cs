/*
 * @Author: fasthro
 * @Date: 2020-12-25 18:15:53
 * @Description:
 */
using Lockstep.MessageData;

namespace Lockstep.Logic
{
    public class Simulator : BaseGameBehaviour
    {
        public bool isRunning { get; private set; }
        public int tick { get; private set; }

        public override void Initialize()
        {
            isRunning = false;
        }

        public void Start(GameStart data)
        {
            int index = 0;
            foreach (var user in data.users)
            {
                var view = _viewService.CreateView<IPlayerView>("Assets/Arts/Player/Player1.prefab", index);
                // TODO userID
                if (user.id == 1) {
                    _gameService.userId = user.id;
                    _gameService.localId = view.entity.cLocalId.id;
                }
                _agentService.CreateAgent(view.entity);
                
                index++;
            }

            isRunning = true;
        }

        public void Step()
        {
            tick++;
        }
    }
}
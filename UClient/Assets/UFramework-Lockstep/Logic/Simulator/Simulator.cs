/*
 * @Author: fasthro
 * @Date: 2020-12-25 18:15:53
 * @Description:
 */

namespace Lockstep.Logic
{
    public class Simulator : BaseBehaviour
    {
        public Simulator(ServiceContainer container) : base(container)
        {
        }

        public bool isRunning { get; private set; }
        public int tick { get; private set; }

        public override void Initialize()
        {
            isRunning = false;
        }

        public void Start(GameStartData data)
        {
            foreach (var user in data.users)
            {
                _viewService.CreateView<IPlayerView>("Assets/Arts/Player/Player1.prefab");
            }

            isRunning = true;
        }

        public void Step()
        {
            tick++;
        }
    }
}
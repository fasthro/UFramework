/*
 * @Author: fasthro
 * @Date: 2020-12-25 18:15:53
 * @Description: 
 */
namespace UFramework.Lockstep
{
    public class Simulator : BaseBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool isRunning { get; private set; }

        /// <summary>
        /// 当前逻辑帧
        /// </summary>
        public int tick { get; private set; }

        /// <summary>
        /// 游戏服务
        /// </summary>
        /// <value></value>
        public IGameService gameService { get; private set; }

        public Simulator(IGameService gameService) : base()
        {
            this.gameService = gameService;
        }

        protected override void OnInitialize()
        {
            isRunning = false;
            tick = 0;
        }

        protected override void OnUpdate(float deltaTime)
        {

        }

        public void StartSimulate()
        {
            isRunning = true;

            var vd = new ViewData();
            vd.artPath = "Assets/Arts/Player/Player1.prefab";
            var player = gameService.gameManager.CreateViewObject<Player>(vd);
            gameService.viewManager.AddPlayer(1, player);
        }

        public void Step()
        {
            tick++;
        }
    }
}
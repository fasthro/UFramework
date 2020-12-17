/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:35:37
 * @Description: Lockstep Launcher
 */

namespace UFramework.Lockstep
{
    [MonoSingletonPath("UFramework-Lockstep")]
    public class LockstepLauncher : MonoSingleton<LockstepLauncher>
    {
        protected override void OnSingletonAwake()
        {
            #region LSTime

            LSTime.Instance.Default();
            Service.Instance.AddUpdateListener(LSTime.OnUpdate, ServiceUpdateOrder.Before);

            #endregion

            #region service

            Service.Instance.RegisterService(new SimulatorService());

            #endregion

            #region manager

            var managerService = Service.Instance.GetService<ManagerService>();

            managerService.RegisterManager(new GameManager());
            managerService.RegisterManager(new BattleManager());
            managerService.RegisterManager(new InputManager());
            managerService.RegisterManager(new FrameManager());

            #endregion
        }

        public void Launched()
        {
            Service.Instance.container.GetService<SimulatorService>().StartSimulate();
        }
    }
}

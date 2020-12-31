/*
 * @Author: fasthro
 * @Date: 2020/12/30 16:38:08
 * @Description:
 */

namespace Lockstep.Logic
{
    public class LauncherClient : Launcher
    {
        public LauncherClient()
        {
            Instance = this;
        }

        public static LauncherClient Instance { get; private set; }

        public override void Update()
        {
            base.Update();
            foreach (var service in _allServices)
                (service as IBehaviour).Update();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var service in _allServices)
                (service as IBehaviour).Dispose();
        }

        protected override void InitializeService()
        {
            base.InitializeService();
            serviceContainer.RegisterService(new ViewService());
            serviceContainer.RegisterService(new NetworkService());
            serviceContainer.RegisterService(new SimulatorService());
            serviceContainer.RegisterService(new InitService());
        }
    }
}
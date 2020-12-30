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

        public void Test()
        {
            serviceContainer.GetService<IInitializeService>().Initialize();
        }

        protected override void InitializeService()
        {
            base.InitializeService();
            serviceContainer.RegisterService(new ViewService());
            serviceContainer.RegisterService(new InitService());
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/30 16:38:08
 * @Description:
 */

namespace Lockstep.Logic
{
    public class LauncherClient : Launcher, ILogger
    {
        public LauncherClient()
        {
            Instance = this;
            Logger.logger = this;
        }

        public static LauncherClient Instance { get; private set; }

        public override void Update()
        {
            base.Update();
            foreach (var service in _allServices)
                service.Update();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var service in _allServices)
                service.Dispose();
        }

        protected override void InitializeService()
        {
            base.InitializeService();
            serviceContainer.RegisterService(new ViewService());
            serviceContainer.RegisterService(new NetworkService());
            serviceContainer.RegisterService(new SimulatorService());
            serviceContainer.RegisterService(new InputService());
            serviceContainer.RegisterService(new InitService());

            _allServices = serviceContainer.GetAllServices();
            foreach (var service in _allServices)
            {
                (service as BaseGameService)?.SetReference();
            }
        }

        public void Debug(object message)
        {
           UnityEngine.Debug.Log(message);
        }

        public void Info(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        public void Warning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public void Error(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
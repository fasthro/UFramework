/*
 * @Author: fasthro
 * @Date: 2020/12/29 20:10:33
 * @Description:
 */

using Entitas;

namespace Lockstep
{
    public abstract class Launcher : IBehaviour
    {
        public ServiceContainer serviceContainer { get; private set; }

        public Contexts contexts { get; private set; }
        public Systems system { get; private set; }

        public void Initialize()
        {
            InitializeService();

            _allServices = serviceContainer.GetAllServices();
            foreach (var service in _allServices)
            {
                var ser = service as BaseService;
                ser.SetReference(serviceContainer);
            }

            InitializeSystem();
        }

        public virtual void Update()
        {
            system.Execute();
            system.Cleanup();
        }

        public virtual void Dispose()
        {
            system.TearDown();
        }

        protected virtual void InitializeService()
        {
            serviceContainer = new ServiceContainer();
            serviceContainer.RegisterService(new GameService());
            serviceContainer.RegisterService(new EntityService());
        }

        protected virtual void InitializeSystem()
        {
            contexts = Contexts.sharedInstance;
            system = new Feature("Systems")
                .Add(new Feature("General")
                    .Add(new InitGameSystem(contexts))
                )
                .Add(new Feature("Movement")
                    .Add(new UpdateViewPositionAndRotationSystem(contexts)));
        }

        private IService[] _allServices;
    }
}
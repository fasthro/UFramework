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
                (service as BaseService).SetReference();
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

        protected IService[] _allServices;

        protected virtual void InitializeService()
        {
            serviceContainer = new ServiceContainer();
            BaseService.SetContainer(serviceContainer);
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
    }
}
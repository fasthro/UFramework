/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:32:13
 * @Description:
 */

namespace Lockstep
{
    public abstract class BaseService : IService
    {
        public BaseService()
        {
            Initialize();
        }

        public static void SetContainer(ServiceContainer container)
        {
            _container = container;
        }

        public virtual void SetReference()
        {
            _gameService = _container.GetService<IGameService>();
            _entityService = _container.GetService<IEntityService>();
            _viewService = _container.GetService<IViewService>();
            _initializeService = _container.GetService<IInitializeService>();
            _networkService = _container.GetService<INetworkService>();
            _initializeService = _container.GetService<IInitializeService>();
            _simulatorService = _container.GetService<ISimulatorService>();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Dispose()
        {
        }

        protected IGameService _gameService;
        protected IEntityService _entityService;
        protected IViewService _viewService;
        protected IInitializeService _initializeService;
        protected ISimulatorService _simulatorService;
        protected INetworkService _networkService;

        private static ServiceContainer _container;
    }
}
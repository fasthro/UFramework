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

        public virtual void SetReference(ServiceContainer container)
        {
            _gameService = container.GetService<IGameService>();
            _entityService = container.GetService<IEntityService>();
            _viewService = container.GetService<IViewService>();
            _initializeService = container.GetService<IInitializeService>();
            _simulatorService = container.GetService<ISimulatorService>();
            _networkService = container.GetService<INetworkService>();
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
    }
}
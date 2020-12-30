/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:32:13
 * @Description:
 */

namespace Lockstep
{
    public abstract class BaseService : IService
    {
        public virtual void SetReference(ServiceContainer container)
        {
            _gameService = container.GetService<IGameService>();
            _entityService = container.GetService<IEntityService>();
            _viewService = container.GetService<IViewService>();
            _initializeService = container.GetService<IInitializeService>();
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
    }
}
﻿// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:32:13
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public abstract class BaseService : IService
    {
        protected static ServiceContainer _container;

        protected IGameService _gameService;
        protected IPlayerService _playerService;
        protected IEntityService _entityService;
        protected IHelperService _helperService;


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
            _playerService = _container.GetService<IPlayerService>();
            _entityService = _container.GetService<IEntityService>();
            _helperService = _container.GetService<IHelperService>();
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
    }
}
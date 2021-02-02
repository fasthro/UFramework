// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------


using GameServer;

namespace UFramework
{
    public abstract class BaseManager : IManager
    {
        protected IHelperManager _helperManager;
        protected IDataManager _dataManager;
        protected IHandlerManager _handlerManager;
        protected IModelManager _modelManager;
        protected INetworkManager _networkManager;

        protected IPlayerManager _playerManager;
        protected IRoomManager _roomManager;

        private static ManagerContainer _container;

        public BaseManager()
        {
            Initialize();
        }

        public static void SetContainer(ManagerContainer container)
        {
            _container = container;
        }

        public virtual void SetReference()
        {
            _helperManager = _container.GetManager<IHelperManager>();
            _dataManager = _container.GetManager<IDataManager>();
            _handlerManager = _container.GetManager<IHandlerManager>();
            _modelManager = _container.GetManager<IModelManager>();
            _networkManager = _container.GetManager<INetworkManager>();
            
            _playerManager = _container.GetManager<IPlayerManager>();
            _roomManager = _container.GetManager<IRoomManager>();
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
/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:22:03
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public abstract class BaseService : IService
    {
        protected static ServiceContainer _container;

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
            _helperService = _container.GetService<IHelperService>();
            _dataService = _container.GetService<IDataService>();
            _handlerService = _container.GetService<IHandlerService>();
            _modelService = _container.GetService<IModelService>();
            _networkService = _container.GetService<INetworkService>();
            //_playerService = _container.GetService<IPlayerService>();
            //_roomService = _container.GetService<IRoomService>();
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

        protected IHelperService _helperService;
        protected IDataService _dataService;
        protected IHandlerService _handlerService;
        protected IModelService _modelService;
        protected INetworkService _networkService;
        //protected IPlayerService _playerService;
        //protected IRoomService _roomService;
    }
}
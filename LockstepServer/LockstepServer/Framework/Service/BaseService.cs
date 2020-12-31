/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:22:03
 * @Description:
 */

using LockstepServer.Src;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public abstract class BaseService : IService
    {
        public BaseService()
        {
            Initialize();
        }

        public virtual void SetReference(ServiceContainer container)
        {
            _helperService = container.GetService<IHelperService>();
            _dataService = container.GetService<IDataService>();
            _handlerService = container.GetService<IHandlerService>();
            _modelService = container.GetService<IModelService>();
            _networkService = container.GetService<INetworkService>();
            _playerService = container.GetService<IPlayerService>();
            _roomService = container.GetService<IRoomService>();
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
        protected IPlayerService _playerService;
        protected IRoomService _roomService;
    }
}
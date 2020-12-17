using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public class Service : Singleton<Service>, ILifeCycle
    {
        public ServiceContainer container { get; set; }

        private BaseService[] _allServices;

        public override void Initialize()
        {
            container = new ServiceContainer();

            RegisterService(new ManagerService());
        }

        #region container

        public void RegisterService(BaseService service, bool overwriteExisting = true)
        {
            container.RegisterService(service, overwriteExisting);

            _allServices = container.GetAllServices();
        }

        public T GetService<T>() where T : BaseService
        {
            return container.GetService<T>();
        }

        public BaseService GetService(string serviceName)
        {
            return container.GetService(serviceName);
        }

        #endregion

        public void DoUpdate(float deltaTime)
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].DoUpdate(deltaTime);
        }

        public void DoDispose()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].DoDispose();
        }
    }
}

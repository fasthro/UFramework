namespace LockstepServer
{
    public class Service : Singleton<Service>
    {
        private IService[] _allServices;
        public ServiceContainer container { get; set; }

        protected override void OnInitialize()
        {
            container = new ServiceContainer();

            RegisterService(new ManagerService());
        }

        protected override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].DoUpdate(deltaTime);
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].DoDispose();
        }

        #region container

        public T GetService<T>() where T : IService
        {
            return container.GetService<T>();
        }

        public IService GetService(string serviceName)
        {
            return container.GetService(serviceName);
        }

        public void RegisterService(IService service, bool overwriteExisting = true)
        {
            container.RegisterService(service, overwriteExisting);

            _allServices = container.GetAllServices();
        }

        #endregion container

        #region manager

        public T GetManager<T>() where T : IManager
        {
            return container.GetService<ManagerService>().GetManager<T>();
        }

        public IManager GetManager(string managerName)
        {
            return container.GetService<ManagerService>().GetManager(managerName);
        }

        #endregion manager
    }
}
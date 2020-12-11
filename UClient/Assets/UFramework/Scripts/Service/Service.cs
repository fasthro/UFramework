/*
 * @Author: fasthro
 * @Date: 2020-12-11 13:38:20
 * @Description: service
 */
namespace UFramework
{
    [MonoSingletonPath("UFramework/Service")]
    public class Service : MonoSingleton<Service>
    {
        public ServiceContainer container { get; set; }

        private BaseService[] _allServices;

        protected override void OnSingletonAwake()
        {
            container = new ServiceContainer();
            container.RegisterService(new ManagerService());

            _allServices = container.GetAllServices();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].OnUpdate(deltaTime);
        }

        protected override void OnSingletonLateUpdate()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].OnLateUpdate();
        }

        protected override void OnSingletonFixedUpdate()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].OnFixedUpdate();
        }

        protected override void OnSingletonDestory()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].OnDestroy();
        }

        protected override void OnSingletonApplicationQuit()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].OnApplicationQuit();
        }
    }
}
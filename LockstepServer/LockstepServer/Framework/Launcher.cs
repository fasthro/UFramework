using log4net;
using log4net.Config;
using log4net.Repository;

/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:44:00
 * @Description:
 */

using System.IO;

namespace LockstepServer
{
    public abstract class Launcher : IBehaviour
    {
        public Launcher(int port)
        {
            _port = port;
        }

        public bool IsRunning => _udpSocketServer.isRunning;
        public ServiceContainer serviceContainer { get; private set; }

        public void Initialize()
        {
            InitializeService();

            _allServices = serviceContainer.GetAllServices();
            foreach (var service in _allServices)
            {
                var ser = service as BaseService;
                ser.SetReference(serviceContainer);
            }

            //创建服务器网络监听
            _udpSocketServer = new UdpSocketServer(serviceContainer.GetService<NetworkService>());
            _udpSocketServer.StartServer(_port);
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void StopServer()
        {
            _udpSocketServer.StopServer();
        }

        public virtual void Update()
        {
            _udpSocketServer.Update();
        }

        public virtual void Dispose()
        {
            _udpSocketServer = null;
        }

        protected IService[] _allServices;

        protected virtual void InitializeService()
        {
            serviceContainer = new ServiceContainer();
            serviceContainer.RegisterService(new HelperService());
            serviceContainer.RegisterService(new NetworkService());
            serviceContainer.RegisterService(new DataService());
            serviceContainer.RegisterService(new HandlerService());
            serviceContainer.RegisterService(new ModelService());
            serviceContainer.RegisterService(new NetworkService());
        }

        private int _port;
        private UdpSocketServer _udpSocketServer;
    }
}
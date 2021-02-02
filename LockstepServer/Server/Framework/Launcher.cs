// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using GameServer;

namespace UFramework
{
    public abstract class Launcher : IBehaviour
    {
        public bool IsRunning => _udpSocketServer != null && _udpSocketServer.isRunning;
        public ManagerContainer managerContainer { get; private set; }

        protected IManager[] _allServices;
        
        private int _port;
        private UdpSocketServer _udpSocketServer;
        

        protected Launcher(int port)
        {
            _port = port;
            Initialize();
        }
        
        public void Initialize()
        {
            InitializeService();

            _allServices = managerContainer.GetAllManagers();
            foreach (var service in _allServices)
                (service as BaseManager)?.SetReference();

            RegisterHandler();

            //创建服务器网络监听
            _udpSocketServer = new UdpSocketServer(managerContainer.GetManager<INetworkManager>() as IServerListener);
            _udpSocketServer.StartServer(_port);
        }
        
        protected virtual void InitializeService()
        {
            managerContainer = new ManagerContainer();
            BaseManager.SetContainer(managerContainer);
            managerContainer.RegisterManager(new HelperManager());
            managerContainer.RegisterManager(new NetworkManager());
            managerContainer.RegisterManager(new DataManager());
            managerContainer.RegisterManager(new HandlerManager());
            managerContainer.RegisterManager(new ModelManager());
            managerContainer.RegisterManager(new NetworkManager());
            
            managerContainer.RegisterManager(new PlayerManager());
            managerContainer.RegisterManager(new RoomManager());
        }
        
        protected virtual void RegisterHandler()
        {
        }

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
    }
}
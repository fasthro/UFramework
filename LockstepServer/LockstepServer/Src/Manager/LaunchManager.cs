/*
 * @Author: fasthro
 * @Date: 2020/12/22 15:44:16
 * @Description:
 */

namespace LockstepServer.Src
{
    public class LaunchManager : BaseManager
    {
        private ManagerService _managerService;
        private HandlerManager _handlerManager;

        public void Launch()
        {
            _managerService = Service.Instance.GetService<ManagerService>();
            _handlerManager = _managerService.GetManager<HandlerManager>();

            RegisterManager();
            RegisterHandler();
        }

        #region manager

        private void RegisterManager()
        {
            RegisterManager<RoomManager>();
        }

        private void RegisterManager<T>() where T : IManager, new()
        {
            _managerService.RegisterManager(new T());
        }

        #endregion manager

        #region handler

        private void RegisterHandler()
        {
            RegisterHandler<HandshakeHandler>();
            RegisterHandler<EnterRoomHandler>();
        }

        private void RegisterHandler<T>() where T : IHandler, new()
        {
            var obj = new T();
            _handlerManager.RegisterHandler(obj.cmd, obj);
        }

        #endregion handler
    }
}
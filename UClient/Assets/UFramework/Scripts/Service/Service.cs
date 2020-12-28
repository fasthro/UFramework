/*
 * @Author: fasthro
 * @Date: 2020-12-11 13:38:20
 * @Description: service
 */
using LuaInterface;
using UFramework.Core;

namespace UFramework
{
    public enum ServiceUpdateOrder
    {
        Before,
        After,
    }

    [MonoSingletonPath("UFramework/Service")]
    public class Service : MonoSingleton<Service>
    {
        public ServiceContainer container { get; set; }

        private event UCallback<float> _updateBeforeListener;
        private event UCallback<float> _updateAfterListener;

        private event UCallback _lateupdateBeforeListener;
        private event UCallback _lateupdateAfterListener;

        private event UCallback _fixedupdateBeforeListener;
        private event UCallback _fixedupdateAfterListener;

        private BaseService[] _allServices;

        protected override void OnSingletonAwake()
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

        [NoToLua]
        public T GetService<T>() where T : BaseService
        {
            return container.GetService<T>();
        }

        public BaseService GetService(string serviceName)
        {
            return container.GetService(serviceName);
        }

        #endregion

        #region listener

        public void AddUpdateListener(UCallback<float> listener, ServiceUpdateOrder order)
        {
            if (order == ServiceUpdateOrder.Before) _updateBeforeListener += listener;
            else _updateAfterListener += listener;
        }

        public void RemoveUpdateListener(UCallback<float> listener, ServiceUpdateOrder order)
        {
            if (order == ServiceUpdateOrder.Before) _updateBeforeListener -= listener;
            else _updateAfterListener -= listener;
        }

        public void AddLateUpdateListener(UCallback listener, ServiceUpdateOrder order)
        {
            if (order == ServiceUpdateOrder.Before) _lateupdateBeforeListener += listener;
            else _lateupdateAfterListener += listener;
        }

        public void RemoveLateUpdateListener(UCallback listener, ServiceUpdateOrder order)
        {
            if (order == ServiceUpdateOrder.Before) _lateupdateBeforeListener -= listener;
            else _lateupdateAfterListener -= listener;
        }

        public void AddFixedUpdateListener(UCallback listener, ServiceUpdateOrder order)
        {
            if (order == ServiceUpdateOrder.Before) _fixedupdateBeforeListener += listener;
            else _fixedupdateAfterListener += listener;
        }

        public void RemoveFixedUpdateListener(UCallback listener, ServiceUpdateOrder order)
        {
            if (order == ServiceUpdateOrder.Before) _fixedupdateBeforeListener -= listener;
            else _fixedupdateAfterListener -= listener;
        }

        #endregion

        protected override void OnSingletonUpdate(float deltaTime)
        {
            _updateBeforeListener?.Invoke(deltaTime);

            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].Update(deltaTime);

            _updateAfterListener?.Invoke(deltaTime);
        }

        protected override void OnSingletonLateUpdate()
        {
            _lateupdateBeforeListener?.Invoke();

            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].LateUpdate();

            _lateupdateAfterListener?.Invoke();
        }

        protected override void OnSingletonFixedUpdate()
        {
            _fixedupdateBeforeListener?.Invoke();

            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].FixedUpdate();

            _fixedupdateAfterListener?.Invoke();
        }

        protected override void OnSingletonDestory()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].Dispose();
        }

        protected override void OnSingletonApplicationQuit()
        {
            for (int i = 0; i < _allServices.Length; i++)
                _allServices[i].ApplicationQuit();
        }
    }
}
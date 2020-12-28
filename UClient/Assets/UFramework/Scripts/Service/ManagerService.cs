/*
 * @Author: fasthro
 * @Date: 2020-12-11 13:38:20
 * @Description: manager service
 */
using LuaInterface;

namespace UFramework
{
    public class ManagerService : BaseService
    {
        public ManagerContainer container { get; set; }

        private BaseManager[] _allManagers;

        protected override void OnInitialize()
        {
            container = new ManagerContainer();

            RegisterManager(new LuaManager());
            RegisterManager(new NetManager());
            RegisterManager(new ResManager());
        }

        public void RegisterManager(BaseManager manager, bool overwriteExisting = true)
        {
            container.RegisterManager(manager, overwriteExisting);

            _allManagers = container.GetAllManagers();
        }

        [NoToLua]
        public T GetManager<T>() where T : BaseManager
        {
            return container.GetManager<T>();
        }

        public BaseManager GetManager(string managerName)
        {
            return container.GetManager(managerName);
        }

        [NoToLua]
        protected override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].Update(deltaTime);
        }

        [NoToLua]
        protected override void OnLateUpdate()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].LateUpdate();
        }

        [NoToLua]
        protected override void OnFixedUpdate()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].FixedUpdate();
        }

        [NoToLua]
        protected override void OnDispose()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].Dispose();
        }

        [NoToLua]
        protected override void OnApplicationQuit()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].ApplicationQuit();
        }
    }
}
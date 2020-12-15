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

        public override void OnAwake()
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
        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnUpdate(deltaTime);
        }

        [NoToLua]
        public override void OnLateUpdate()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnLateUpdate();
        }

        [NoToLua]
        public override void OnFixedUpdate()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnFixedUpdate();
        }

        [NoToLua]
        public override void OnDestroy()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnDestroy();
        }

        [NoToLua]
        public override void OnApplicationQuit()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnApplicationQuit();
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-12-11 13:38:20
 * @Description: manager service
 */
namespace UFramework
{
    public class ManagerService : BaseService
    {
        public ManagerContainer container { get; set; }

        private BaseManager[] _allManagers;

        public override void OnAwake()
        {
            container = new ManagerContainer();
            container.RegisterManager(new LuaManager());
            container.RegisterManager(new NetManager());
            container.RegisterManager(new ResManager());

            _allManagers = container.GetAllManagers();
        }

        public override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnUpdate(deltaTime);
        }

        public override void OnLateUpdate()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnLateUpdate();
        }

        public override void OnFixedUpdate()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnFixedUpdate();
        }

        public override void OnDestroy()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnDestroy();
        }

        public override void OnApplicationQuit()
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].OnApplicationQuit();
        }
    }
}
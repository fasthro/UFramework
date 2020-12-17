using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public class ManagerService : BaseService
    {

        public ManagerContainer container { get; set; }

        private BaseManager[] _allManagers;

        protected override void OnInitialize()
        {
            container = new ManagerContainer();

        }

        public void RegisterManager(BaseManager manager, bool overwriteExisting = true)
        {
            container.RegisterManager(manager, overwriteExisting);

            _allManagers = container.GetAllManagers();
        }

        public T GetManager<T>() where T : BaseManager
        {
            return container.GetManager<T>();
        }

        public BaseManager GetManager(string managerName)
        {
            return container.GetManager(managerName);
        }

        protected override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].DoUpdate(deltaTime);
        }
    }
}

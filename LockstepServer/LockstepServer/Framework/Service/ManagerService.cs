using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public class ManagerService : BaseService
    {
        private IManager[] _allManagers;
        public ManagerContainer container { get; set; }

        #region base

        protected override void OnInitialize()
        {
            container = new ManagerContainer();

            RegisterManager(new NetManager());
            RegisterManager(new ModelManager());
            RegisterManager(new DataManager());
        }

        protected override void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _allManagers.Length; i++)
                _allManagers[i].DoUpdate(deltaTime);
        }

        #endregion base

        #region api

        public T GetManager<T>() where T : IManager
        {
            return container.GetManager<T>();
        }

        public IManager GetManager(string managerName)
        {
            return container.GetManager(managerName);
        }

        public void RegisterManager(IManager manager, bool overwriteExisting = true)
        {
            container.RegisterManager(manager, overwriteExisting);

            _allManagers = container.GetAllManagers();
        }

        #endregion api
    }
}
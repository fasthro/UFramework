using System;
using System.Collections.Generic;
using System.Linq;

namespace LockstepServer
{
    public class ManagerContainer
    {
        private Dictionary<Type, BaseManager> _allManagers = new Dictionary<Type, BaseManager>();
        private Dictionary<string, Type> _allManagerTypes = new Dictionary<string, Type>();

        public void RegisterManager(BaseManager manager, bool overwriteExisting = true)
        {
            var type = manager.GetType();
            if (!_allManagers.ContainsKey(type))
            {
                _allManagers.Add(type, manager);
                _allManagerTypes.Add(type.Name, type);
            }
            else if (overwriteExisting)
            {
                _allManagers[type] = manager;
                _allManagerTypes[type.Name] = type;
            }
        }

        public BaseManager[] GetAllManagers()
        {
            return _allManagers.Values.ToArray();
        }

        public T GetManager<T>() where T : BaseManager
        {
            var key = typeof(T);
            if (_allManagers.ContainsKey(key))
            {
                return (T)_allManagers[key];
            }
            return default(T);
        }

        public BaseManager GetManager(string managerName)
        {
            if (_allManagerTypes.ContainsKey(managerName))
            {
                return _allManagers[_allManagerTypes[managerName]];
            }
            return null;
        }
    }
}
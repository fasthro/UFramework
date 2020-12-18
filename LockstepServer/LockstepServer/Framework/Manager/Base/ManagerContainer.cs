using System;
using System.Collections.Generic;
using System.Linq;

namespace LockstepServer
{
    public class ManagerContainer
    {
        private Dictionary<Type, IManager> _allManagers = new Dictionary<Type, IManager>();
        private Dictionary<string, Type> _allManagerTypes = new Dictionary<string, Type>();

        public IManager[] GetAllManagers()
        {
            return _allManagers.Values.ToArray();
        }

        public T GetManager<T>() where T : IManager
        {
            var key = typeof(T);
            if (_allManagers.ContainsKey(key))
            {
                return (T)_allManagers[key];
            }
            return default(T);
        }

        public IManager GetManager(string managerName)
        {
            if (_allManagerTypes.ContainsKey(managerName))
            {
                return _allManagers[_allManagerTypes[managerName]];
            }
            return null;
        }

        public void RegisterManager(IManager manager, bool overwriteExisting = true)
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
    }
}
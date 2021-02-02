// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace UFramework
{
    public class ManagerContainer
    {
        private Dictionary<Type, IManager> _allManagerDict = new Dictionary<Type, IManager>();

        public void RegisterManager(IManager manager)
        {
            var interfaceTypes = manager.GetType().FindInterfaces((type, criteria) =>
                    type.GetInterfaces().Any(t => t == typeof(IManager)), manager)
                .ToArray();

            foreach (var type in interfaceTypes)
            {
                if (!_allManagerDict.ContainsKey(type))
                    _allManagerDict.Add(type, manager);
                else _allManagerDict[type] = manager;
            }
        }

        public T GetManager<T>() where T : IManager
        {
            var key = typeof(T);
            if (_allManagerDict.ContainsKey(key))
                return (T) _allManagerDict[key];
            return default(T);
        }

        public IManager[] GetAllManagers() => _allManagerDict.Values.ToArray();
    }
}
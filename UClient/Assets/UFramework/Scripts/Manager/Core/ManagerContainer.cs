/*
 * @Author: fasthro
 * @Date: 2020-12-11 14:38:35
 * @Description: 服务容器
 */
using System;
using System.Collections.Generic;
using System.Linq;
using LuaInterface;

namespace UFramework
{
    public class ManagerContainer
    {
        private Dictionary<Type, BaseManager> _allManagers = new Dictionary<Type, BaseManager>();
        private Dictionary<string, Type> _allManagerTypes = new Dictionary<string, Type>();

        [NoToLua]
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

        [NoToLua]
        public BaseManager[] GetAllManagers()
        {
            return _allManagers.Values.ToArray();
        }

        [NoToLua]
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
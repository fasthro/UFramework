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
    public class ServiceContainer
    {
        private Dictionary<Type, BaseService> _allServices = new Dictionary<Type, BaseService>();
        private Dictionary<string, Type> _allServiceTypes = new Dictionary<string, Type>();

        [NoToLua]
        public void RegisterService(BaseService service, bool overwriteExisting = true)
        {
            var type = service.GetType();
            if (!_allServices.ContainsKey(type))
            {
                _allServices.Add(type, service);
                _allServiceTypes.Add(type.Name, type);
            }
            else if (overwriteExisting)
            {
                _allServices[type] = service;
                _allServiceTypes[type.Name] = type;
            }
        }

        [NoToLua]
        public BaseService[] GetAllServices()
        {
            return _allServices.Values.ToArray();
        }

        [NoToLua]
        public T GetService<T>() where T : BaseService
        {
            var key = typeof(T);
            if (_allServices.ContainsKey(key))
            {
                return (T)_allServices[key];
            }
            return default(T);
        }

        public BaseService GetService(string serviceName)
        {
            if (_allServiceTypes.ContainsKey(serviceName))
            {
                return _allServices[_allServiceTypes[serviceName]];
            }
            return null;
        }
    }
}
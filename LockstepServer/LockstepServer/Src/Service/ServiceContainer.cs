using System;
using System.Collections.Generic;
using System.Linq;

namespace LockstepServer
{
    public class ServiceContainer
    {
        private Dictionary<Type, BaseService> _allServices = new Dictionary<Type, BaseService>();
        private Dictionary<string, Type> _allServiceTypes = new Dictionary<string, Type>();

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

        public BaseService[] GetAllServices()
        {
            return _allServices.Values.ToArray();
        }

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
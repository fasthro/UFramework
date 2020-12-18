using System;
using System.Collections.Generic;
using System.Linq;

namespace LockstepServer
{
    public class ServiceContainer
    {
        private Dictionary<Type, IService> _allServices = new Dictionary<Type, IService>();
        private Dictionary<string, Type> _allServiceTypes = new Dictionary<string, Type>();

        public IService[] GetAllServices()
        {
            return _allServices.Values.ToArray();
        }

        public T GetService<T>() where T : IService
        {
            var key = typeof(T);
            if (_allServices.ContainsKey(key))
            {
                return (T)_allServices[key];
            }
            return default(T);
        }

        public IService GetService(string serviceName)
        {
            if (_allServiceTypes.ContainsKey(serviceName))
            {
                return _allServices[_allServiceTypes[serviceName]];
            }
            return null;
        }

        public void RegisterService(IService service, bool overwriteExisting = true)
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
    }
}
/*
 * @Author: fasthro
 * @Date: 2020/12/29 19:52:10
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lockstep
{
    public class ServiceContainer
    {
        public void RegisterService(IService service)
        {
            var type = service.GetType();
            if (!_allServices.ContainsKey(type))
            {
                _allServices.Add(type, service);
            }
            else
            {
                _allServices[type] = service;
            }
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

        public IService[] GetAllServices()
        {
            return _allServices.Values.ToArray();
        }

        private Dictionary<Type, IService> _allServices = new Dictionary<Type, IService>();
    }
}
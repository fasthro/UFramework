/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: Singleton 创建器
 */

using System;
using System.Reflection;

namespace UFramework
{
    public static class SingletonCreator
    {
        public static T CreateSingleton<T>() where T : class, ISingleton
        {
            // 获取私有构造函数
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            // 获取无参构造函数
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
            if (ctor == null)
                throw new Exception("Non-Public Constructor() not found! in " + typeof(T));

            // 通过构造函数，常见实例
            var retInstance = ctor.Invoke(null) as T;
            retInstance.SingletonAwake();

            return retInstance;
        }
    }
}
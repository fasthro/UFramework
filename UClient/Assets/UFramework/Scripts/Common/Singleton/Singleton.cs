/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: Singleton
 */

namespace UFramework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        protected static T instance;
        private static object obj = new object();

        public static T Instance
        {
            get
            {
                if (null == Singleton<T>.instance)
                {
                    lock (obj)
                    {
                        if (null == Singleton<T>.instance)
                        {
                            Singleton<T>.instance = System.Activator.CreateInstance<T>();
                            (Singleton<T>.instance as Singleton<T>).SingletonAwake();
                        }
                    }
                }
                return instance;
            }
        }

        public void SingletonAwake() { OnSingletonAwake(); }

        protected virtual void OnSingletonAwake() { }
        protected virtual void OnSingletonDestory() { }

        public virtual void Dispose()
        {
            if (Singleton<T>.instance != null)
            {
                Singleton<T>.instance = (T)((object)null);
            }
            OnSingletonDestory();
        }

        public static bool HasInstance() { return Singleton<T>.instance != null; }
    }
}
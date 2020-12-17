using System;

namespace LockstepServer
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
                            (Singleton<T>.instance as Singleton<T>).Initialize();
                        }
                    }
                }
                return instance;
            }
        }

        public void Default() { }

        public virtual void Initialize() {  }

        public virtual void Dispose()
        {
            if (Singleton<T>.instance != null)
            {
                Singleton<T>.instance = (T)((object)null);
            }
        }

        public static bool HasInstance() { return Singleton<T>.instance != null; }
    }
}
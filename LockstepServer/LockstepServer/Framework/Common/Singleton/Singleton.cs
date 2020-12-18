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

        public static bool HasInstance()
        {
            return Singleton<T>.instance != null;
        }

        public void Default()
        {
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public void DoUpdate(float deltaTime)
        {
            OnUpdate(deltaTime);
        }

        public void DoDispose()
        {
            OnDispose();
            Dispose();
        }

        public void Dispose()
        {
            OnDispose();
            if (Singleton<T>.instance != null)
            {
                Singleton<T>.instance = (T)((object)null);
            }
        }

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }
    }
}
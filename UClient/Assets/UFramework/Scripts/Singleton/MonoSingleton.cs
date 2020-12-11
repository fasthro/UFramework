/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: MonoSingleton
 */
using UnityEngine;

namespace UFramework
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static T instance = null;
        private static readonly object obj = new object();
        private static bool isDestory = false;

        public static T Instance
        {
            get
            {
                if (isDestory)
                {
                    Logger.Error(string.Format("Try To Call [MonoSingleton] Instance {0} When The Application Already Quit, return null inside", typeof(T)));
                    return null;
                }
                lock (obj)
                {
                    if (null == instance)
                    {
                        instance = MonoSingletonCreator.CreateMonoSingleton<T>();
                    }
                }
                return MonoSingleton<T>.instance;
            }
        }

        void Awake()
        {
            isDestory = false;
            SingletonAwake();
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            OnSingletonStart();
        }

        void Update()
        {
            OnSingletonUpdate(Time.deltaTime);
        }

        void LateUpdate()
        {
            OnSingletonLateUpdate();
        }

        void FixedUpdate()
        {
            OnSingletonFixedUpdate();
        }

        void OnDestroy()
        {
            isDestory = true;
            if (MonoSingleton<T>.instance != null && MonoSingleton<T>.instance.gameObject == base.gameObject)
            {
                MonoSingleton<T>.instance = (T)((object)null);
            }
            OnSingletonDestory();
        }

        void OnApplicationQuit()
        {
            OnSingletonApplicationQuit();
        }

        public void Default() { }

        public void SingletonAwake() { OnSingletonAwake(); }

        protected virtual void OnSingletonAwake() { }

        protected virtual void OnSingletonStart() { }

        protected virtual void OnSingletonDestory() { }
        protected virtual void OnSingletonApplicationQuit() { }

        protected virtual void OnSingletonUpdate(float deltaTime) { }
        protected virtual void OnSingletonLateUpdate() { }
        protected virtual void OnSingletonFixedUpdate() { }


        public virtual void Dispose()
        {
            MonoSingleton<T>.instance = null;
            Destroy(gameObject);
        }

        public static bool HasInstance() { return MonoSingleton<T>.instance != null; }
    }
}
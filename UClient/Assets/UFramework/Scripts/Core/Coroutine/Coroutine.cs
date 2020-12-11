/*
 * @Author: fasthro
 * @Date: 2019-10-24 17:08:12
 * @Description: 协同程序包装 (支持停止，暂停，取消暂停)，扩展了Unity自带的协同程序
 */
using System.Collections;
using UFramework.Core;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Core
{
    public class CoroutineHelper : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public class Coroutine : IPoolBehaviour
    {
        static CoroutineHelper _Helper;
        static CoroutineHelper Helper
        {
            get
            {
                if (_Helper == null)
                    _Helper = (new GameObject("CoroutineHelper")).AddComponent<CoroutineHelper>();
                return _Helper;
            }
        }

        public bool running { get; private set; }
        public bool paused { get; private set; }
        public bool stopped { get; private set; }

        private UCallback<bool> _completeCallback;
        private IEnumerator _routine;

        #region pool

        public bool isRecycled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="Start"></param>
        /// <param name="completeCallback"></param>
        /// <returns></returns>
        public static Coroutine Allocate(IEnumerator routine, bool autoStart = true, UCallback<bool> completeCallback = null)
        {
            return ObjectPool<Coroutine>.Instance.Allocate().Initialize(routine, autoStart, completeCallback);
        }

        public void Recycle()
        {
            ObjectPool<Coroutine>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            if (_routine != null)
                Helper.StopCoroutine(_routine);
            _routine = null;

            _completeCallback = null;
        }

        #endregion

        private Coroutine Initialize(IEnumerator routine, bool autoStart, UCallback<bool> completeCallback = null)
        {
            _routine = routine;
            _completeCallback = completeCallback;
            if (autoStart)
            {
                Start();
            }
            return this;
        }

        public void Start()
        {
            running = true;
            Helper.StartCoroutine(CallWrapper());
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public void Stop()
        {
            stopped = true;
            running = false;
        }

        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = this._routine;
            while (running)
            {
                if (paused)
                    yield return null;
                else
                {
                    if (e != null && e.MoveNext()) yield return e.Current;
                    else running = false;
                }
            }
            _completeCallback.InvokeGracefully(stopped);
        }
    }
}
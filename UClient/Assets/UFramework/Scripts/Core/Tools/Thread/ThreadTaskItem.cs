/*
 * @Author: fasthro
 * @Date: 2020-09-23 10:51:24
 * @Description: thread task item
 */
using System;
using UFramework.Pool;

namespace UFramework.Tools
{
    public class ThreadTaskItem : IPoolObject
    {
        private Action _callback0;
        private Action<object> _callback1;
        private Action<object, object> _callback2;
        private Action<object, object, object> _callback3;

        private object _object1;
        private object _object2;
        private object _object3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ThreadTaskItem Allocate(Action callback)
        {
            return ObjectPool<ThreadTaskItem>.Instance.Allocate().Builder(callback); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ThreadTaskItem Allocate(Action<object> callback, object param)
        {
            return ObjectPool<ThreadTaskItem>.Instance.Allocate().Builder(callback, param); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public static ThreadTaskItem Allocate(Action<object, object> callback, object param1, object param2)
        {
            return ObjectPool<ThreadTaskItem>.Instance.Allocate().Builder(callback, param1, param2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <returns></returns>
        public static ThreadTaskItem Allocate(Action<object, object, object> callback, object param1, object param2, object param3)
        {
            return ObjectPool<ThreadTaskItem>.Instance.Allocate().Builder(callback, param1, param2, param3);
        }

        #region IPoolObject

        public bool isRecycled { get; set; }

        public void Recycle()
        {
            ObjectPool<ThreadTaskItem>.Instance.Recycle(this);
        }

        public void OnRecycle()
        {
            _callback0 = null;
            _callback1 = null;
            _callback2 = null;
            _callback3 = null;
        }

        #endregion

        private ThreadTaskItem Builder(Action callback)
        {
            _callback0 = callback;
            return this;
        }

        private ThreadTaskItem Builder(Action<object> callback, object param)
        {
            _callback1 = callback;
            return this;
        }

        private ThreadTaskItem Builder(Action<object, object> callback, object param, object param2)
        {
            _callback2 = callback;
            return this;
        }

        private ThreadTaskItem Builder(Action<object, object, object> callback, object param1, object param2, object param3)
        {
            _callback3 = callback;
            return this;
        }

        public ThreadTaskItem Execute()
        {
            _callback0.InvokeGracefully();
            _callback1.InvokeGracefully(_object1);
            _callback2.InvokeGracefully(_object1, _object2);
            _callback3.InvokeGracefully(_object2, _object2, _object3);
            return this;
        }
    }
}
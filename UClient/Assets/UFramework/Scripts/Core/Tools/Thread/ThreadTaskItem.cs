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
        private Action m_callback0;
        private Action<object> m_callback1;
        private Action<object, object> m_callback2;
        private Action<object, object, object> m_callback3;

        private object m_object1;
        private object m_object2;
        private object m_object3;

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
            m_callback0 = null;
            m_callback1 = null;
            m_callback2 = null;
            m_callback3 = null;
        }

        #endregion

        private ThreadTaskItem Builder(Action callback)
        {
            m_callback0 = callback;
            return this;
        }

        private ThreadTaskItem Builder(Action<object> callback, object param)
        {
            m_callback1 = callback;
            return this;
        }

        private ThreadTaskItem Builder(Action<object, object> callback, object param, object param2)
        {
            m_callback2 = callback;
            return this;
        }

        private ThreadTaskItem Builder(Action<object, object, object> callback, object param1, object param2, object param3)
        {
            m_callback3 = callback;
            return this;
        }

        public ThreadTaskItem Execute()
        {
            m_callback0.InvokeGracefully();
            m_callback1.InvokeGracefully(m_object1);
            m_callback2.InvokeGracefully(m_object1, m_object2);
            m_callback3.InvokeGracefully(m_object2, m_object2, m_object3);
            return this;
        }
    }
}
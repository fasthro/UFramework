/*
 * @Author: fasthro
 * @Date: 2020-06-15 22:56:20
 * @Description: 线程任务队列
 * 1.线程执行任务 
 *   Enqueue(Action taskFunc, Action completeFunc = null, int sleep = 0)
 * 2.主线程执行任务
 *   EnqueueMain(Action taskFunc)
 *   EnqueueMain(Action<object> taskFunc, object param)
 *   EnqueueMain(Action<object, object> taskFunc, object param1, object param2)
 *   EnqueueMain(Action<object, object, object> taskFunc, object param1, object param2, object param3)
 */

using System;
using System.Collections.Generic;
using System.Threading;
using UFramework.Pool;

namespace UFramework.Tools
{
    public class ThreadQueue : MonoSingleton<ThreadQueue>
    {
        // 主线程ID
        public static int mainThreadId { get; private set; }

        /// <summary>
        /// 是否为主线程
        /// </summary>
        /// <returns></returns>
        public static bool isMainThread
        {
            get
            {
                return GetCurrentThreadId() == mainThreadId;
            }
        }

        /// <summary>
        /// 主线程任务
        /// </summary>
        public class MainTaskItem : IPoolObject
        {
            private Action m_callback0;
            private Action<object> m_callback1;
            private Action<object, object> m_callback2;
            private Action<object, object, object> m_callback3;

            private object m_object1;
            private object m_object2;
            private object m_object3;

            public static MainTaskItem Allocate(Action callback)
            {
                var obj = ObjectPool<MainTaskItem>.Instance.Allocate();
                obj.SetAction(callback);
                return obj;
            }

            public static MainTaskItem Allocate(Action<object> callback, object param)
            {
                var obj = ObjectPool<MainTaskItem>.Instance.Allocate();
                obj.SetAction(callback, param);
                return obj;
            }

            public static MainTaskItem Allocate(Action<object, object> callback, object param1, object param2)
            {
                var obj = ObjectPool<MainTaskItem>.Instance.Allocate();
                obj.SetAction(callback, param1, param2);
                return obj;
            }

            public static MainTaskItem Allocate(Action<object, object, object> callback, object param1, object param2, object param3)
            {
                var obj = ObjectPool<MainTaskItem>.Instance.Allocate();
                obj.SetAction(callback, param1, param2, param3);
                return obj;
            }

            #region IPoolObject

            public bool isRecycled { get; set; }

            public void Recycle()
            {
                ObjectPool<MainTaskItem>.Instance.Recycle(this);
            }

            public void OnRecycle()
            {
                m_callback0 = null;
                m_callback1 = null;
                m_callback2 = null;
                m_callback3 = null;
            }

            #endregion

            private void SetAction(Action callback)
            {
                m_callback0 = callback;
            }

            private void SetAction(Action<object> callback, object param)
            {
                m_callback1 = callback;
            }

            private void SetAction(Action<object, object> callback, object param, object param2)
            {
                m_callback2 = callback;
            }

            private void SetAction(Action<object, object, object> callback, object param1, object param2, object param3)
            {
                m_callback3 = callback;
            }

            public void Execute()
            {
                m_callback0.InvokeGracefully();
                m_callback1.InvokeGracefully(m_object1);
                m_callback2.InvokeGracefully(m_object1, m_object2);
                m_callback3.InvokeGracefully(m_object2, m_object2, m_object3);
            }
        }

        /// <summary>
        /// 主线程任务队列
        /// </summary>
        /// <typeparam name="MainTaskItem"></typeparam>
        /// <returns></returns>
        private static DoubleQueue<MainTaskItem> mainTaskQueue = new DoubleQueue<MainTaskItem>(16);
    
        protected override void OnSingletonAwake()
        {
            mainThreadId = GetCurrentThreadId();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (mainTaskQueue.IsEmpty())
            {
                mainTaskQueue.Swap();
            }
            while (!mainTaskQueue.IsEmpty())
            {
                var task = mainTaskQueue.Dequeue();
                task.Execute();
                task.Recycle();
            }
        }

        protected override void OnSingletonDestory()
        {
            mainTaskQueue.Clear();
        }

        /// <summary>
        /// 获取当前线程ID
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// 线程任务入列
        /// </summary>
        /// <param name="taskFunc">任务</param>
        /// <param name="completeFunc">任务完成回调</param>
        /// <param name="sleep">延时执行任务（单位：milliseconds）</param>
        public static void Enqueue(Action taskFunc, Action completeFunc = null, int sleep = 0)
        {
            WaitCallback wc = (state) =>
            {
                if (sleep > 0)
                {
                    Sleep(sleep);
                }
                taskFunc();
                completeFunc.InvokeGracefully();
            };

            ThreadPool.QueueUserWorkItem(wc);
        }

        /// <summary>
        /// 线程休眠
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        public static void EnqueueMain(Action taskFunc)
        {
            mainTaskQueue.Enqueue(MainTaskItem.Allocate(taskFunc));
        }

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="param"></param>
        public static void EnqueueMain(Action<object> taskFunc, object param)
        {
            mainTaskQueue.Enqueue(MainTaskItem.Allocate(taskFunc, param));
        }

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public static void EnqueueMain(Action<object, object> taskFunc, object param1, object param2)
        {
            mainTaskQueue.Enqueue(MainTaskItem.Allocate(taskFunc, param1, param2));
        }

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        public static void EnqueueMain(Action<object, object, object> taskFunc, object param1, object param2, object param3)
        {
            mainTaskQueue.Enqueue(MainTaskItem.Allocate(taskFunc, param1, param2, param3));
        }
    }
}
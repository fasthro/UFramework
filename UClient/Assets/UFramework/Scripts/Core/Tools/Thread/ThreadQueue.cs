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
using System.Threading;

namespace UFramework.Tools
{
    [MonoSingletonPath("UFramework/ThreadQueue")]
    public class ThreadQueue : MonoSingleton<ThreadQueue>
    {
        // 主线程ID
        public static int MainThreadId { get; private set; }

        /// <summary>
        /// 是否为主线程
        /// </summary>
        /// <returns></returns>
        public static bool IsMainThread
        {
            get
            {
                return GetCurrentThreadId() == MainThreadId;
            }
        }

        /// <summary>
        /// 主线程任务队列
        /// </summary>
        /// <typeparam name="MainTaskItem"></typeparam>
        /// <returns></returns>
        static DoubleQueue<ThreadTaskItem> MainTaskQueue = new DoubleQueue<ThreadTaskItem>(16);

        protected override void OnSingletonAwake()
        {
            MainThreadId = GetCurrentThreadId();
        }

        /// <summary>
        /// 获取当前线程ID
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentThreadId()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        #region 线程任务
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

        #endregion

        #region 主线程任务

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        public static void EnqueueMain(Action taskFunc)
        {
            MainTaskQueue.Enqueue(ThreadTaskItem.Allocate(taskFunc));
        }

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="param"></param>
        public static void EnqueueMain(Action<object> taskFunc, object param)
        {
            MainTaskQueue.Enqueue(ThreadTaskItem.Allocate(taskFunc, param));
        }

        /// <summary>
        /// 主线程任务入列
        /// </summary>
        /// <param name="taskFunc"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public static void EnqueueMain(Action<object, object> taskFunc, object param1, object param2)
        {
            MainTaskQueue.Enqueue(ThreadTaskItem.Allocate(taskFunc, param1, param2));
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
            MainTaskQueue.Enqueue(ThreadTaskItem.Allocate(taskFunc, param1, param2, param3));
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (MainTaskQueue.IsEmpty())
            {
                MainTaskQueue.Swap();
            }
            while (!MainTaskQueue.IsEmpty())
            {
                MainTaskQueue.Dequeue().Execute().Recycle();
            }
        }

        protected override void OnSingletonDestory()
        {
            MainTaskQueue.Clear();
        }

        #endregion
    }
}
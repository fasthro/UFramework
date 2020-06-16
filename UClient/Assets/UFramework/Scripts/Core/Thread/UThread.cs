/*
 * @Author: fasthro
 * @Date: 2020-06-15 22:56:20
 * @Description: 线程
 */

using System;
using System.Threading;
using thread = System.Threading.Thread;

namespace UFramework.Thread
{
    public static class UThread
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
        /// 线程初始化
        /// </summary>
        public static void Initialize()
        {
            mainThreadId = GetCurrentThreadId();
        }

        /// <summary>
        /// 获取当前线程ID
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentThreadId()
        {
            return thread.CurrentThread.ManagedThreadId;
        }


        /// <summary>
        /// 在线程中执行方法
        /// </summary>
        /// <param name="taskFunc">任务</param>
        /// <param name="completeFunc">任务完成回调</param>
        /// <param name="sleep">延时执行任务（单位：milliseconds）</param>
        public static void Queue(Action taskFunc, Action completeFunc = null, int sleep = 0)
        {
            WaitCallback wc = (state) =>
            {
                if (sleep > 0)
                {
                    Sleep(sleep);
                }

                taskFunc();

                // TODO
                if (completeFunc != null)
                {
                    completeFunc();
                }
            };

            ThreadPool.QueueUserWorkItem(wc);
        }

        /// <summary>
        /// 线程休眠
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void Sleep(int milliseconds)
        {
            thread.Sleep(milliseconds);
        }
    }
}
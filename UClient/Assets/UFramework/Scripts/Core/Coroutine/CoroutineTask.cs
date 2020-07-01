/*
 * @Author: fasthro
 * @Date: 2020-07-01 16:10:18
 * @Description: 协同程序任务
 */

using System.Collections;
using System.Collections.Generic;

namespace UFramework.Coroutine
{
    /// <summary>
    /// 协同任务执行者接口
    /// </summary>
    public interface IUCoroutineTaskRunner
    {
        IEnumerator OnCoroutineTaskRun();
    }

    /// <summary>
    /// 协同任务管理
    /// </summary>
    public class UCoroutineTask
    {
        /// <summary>
        /// 任务最大执行数量
        /// </summary>
        private const int maxRunCount = 8;

        /// <summary>
        /// 当前任务数量
        /// </summary>
        private static int runCount;

        /// <summary>
        /// 任务执行者列表
        /// </summary>
        /// <typeparam name="ICoroutineTaskRunner"></typeparam>
        /// <returns></returns>
        private static LinkedList<IUCoroutineTaskRunner> runners = new LinkedList<IUCoroutineTaskRunner>();

        /// <summary>
        /// Add Task Runner
        /// </summary>
        /// <param name="runner"></param>
        public static void AddTaskRunner(IUCoroutineTaskRunner runner)
        {
            runners.AddLast(runner);
            TryRun();
        }

        /// <summary>
        /// Try Run Task
        /// </summary>
        private static void TryRun()
        {
            if (runners.Count == 0) return;
            if (runCount >= maxRunCount) return;

            var runner = runners.First.Value;
            runners.RemoveFirst();

            ++runCount;
            UFactoryCoroutine.CreateRun(runner.OnCoroutineTaskRun());
        }

        /// <summary>
        /// Try Next Run Task
        /// </summary>
        private static void TryNextRun()
        {
            --runCount;
            TryRun();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void TaskComplete()
        {
            TryNextRun();
        }
    }
}
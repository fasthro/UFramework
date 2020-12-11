/*
 * @Author: fasthro
 * @Date: 2020-12-01 16:51:34
 * @Description: 
 */
using System.Collections;
using System.Collections.Generic;

namespace UFramework.Core
{
    public interface ICoroutineWork
    {
        IEnumerator DoCoroutineWork();
    }

    public static class CoroutineWorker
    {
        const int MAX_WORK_COUNT = 8;

        static int CurWorkCount;
        static Queue<ICoroutineWork> Works = new Queue<ICoroutineWork>();

        public static void Push(ICoroutineWork work)
        {
            Works.Enqueue(work);
            DoWork();
        }

        static void DoWork()
        {
            if (Works.Count == 0) return;
            if (CurWorkCount >= MAX_WORK_COUNT) return;

            CurWorkCount++;
            Coroutine.Allocate(Works.Dequeue().DoCoroutineWork(), true, OnCompleted);
        }

        static void DoNextWork()
        {
            CurWorkCount--;
            DoWork();
        }

        static void OnCompleted(bool result)
        {
            DoNextWork();
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2019-10-24 16:55:36
 * @Description: Coroutine 协同程序工厂
 */

using System.Collections;
using UFramework.Messenger;

namespace UFramework.Coroutine
{
    public static class UFactoryCoroutine
    {
        /// <summary>
        /// 创建协同程序
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="completeCallback"></param>
        /// <returns></returns>
        public static UCoroutine Create(IEnumerator routine, UCallback<bool> completeCallback = null)
        {
            return new UCoroutine(routine, false, completeCallback);
        }

        /// <summary>
        /// 创建协同程序并且自动执行
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="completeCallback"></param>
        /// <returns></returns>
        public static UCoroutine CreateRun(IEnumerator routine, UCallback<bool> completeCallback = null)
        {
            return new UCoroutine(routine, true, completeCallback);
        }
    }
}
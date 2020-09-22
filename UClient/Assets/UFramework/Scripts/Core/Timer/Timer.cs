/*
 * @Author: fasthro
 * @Date: 2020-09-22 16:05:58
 * @Description: timer
 */
using System.Collections.Generic;
using LuaInterface;
using UFramework.Messenger;

namespace UFramework.Timers
{
    [MonoSingletonPath("UFramework/Timer")]
    public class Timer : MonoSingleton<Timer>
    {
        readonly static List<TimerEntity> timers = new List<TimerEntity>();

        /// <summary>
        /// 类似双向缓冲队列，不会阻塞timers运行
        /// </summary>
        /// <typeparam name="TImerEntity"></typeparam>
        /// <returns></returns>
        readonly static List<TimerEntity> timersBuffer = new List<TimerEntity>();

        readonly static List<int> removes = new List<int>();

        /// <summary>
        /// add timer
        /// </summary>
        /// <param name="duration">在计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="onUpdate">每帧更新事件</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="useRealTime">是否使用真实时间不受时间缩放影响</param>
        /// <returns></returns>
        public static TimerEntity Add(float duration, UCallback onComplete, UCallback<float> onUpdate = null, bool isLooped = false, bool useRealTime = false)
        {
            var entity = TimerEntity.Allocate(duration, onComplete, onUpdate, isLooped, useRealTime);
            timersBuffer.Add(entity);
            return entity;
        }

        /// <summary>
        /// lua add timer
        /// </summary>
        /// <param name="duration">在计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="onUpdate">每帧更新事件</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="useRealTime">是否使用真实时间不受时间缩放影响</param>
        /// <returns></returns>
        public static TimerEntity Add(float duration, LuaFunction onComplete, LuaTable self = null, bool isLooped = false, bool useRealTime = false)
        {
            var entity = LuaTimerEntity.LuaAllocate(duration, onComplete, self, isLooped, useRealTime);
            timersBuffer.Add(entity);
            return entity;
        }

        /// <summary>
        /// 取消所有定时器
        /// </summary>
        public static void CancelAll()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Cancel();
            }
        }

        /// <summary>
        /// 暂停所有定时器
        /// </summary>
        public static void PauseAll()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Pause();
            }
        }

        /// <summary>
        /// 唤醒所有定时器
        /// </summary>
        public static void ResumeAll()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Resume();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void OnSingletonUpdate(float deltaTime)
        {
            if (timersBuffer.Count > 0)
            {
                timers.AddRange(timersBuffer);
                timersBuffer.Clear();
            }

            removes.Clear();
            for (int i = 0; i < timers.Count; i++)
            {
                var timer = timers[i];
                timer.OnUpdate();
                if (timer.isIneffective)
                {
                    timer.Recycle();
                    removes.Add(i);
                }
            }
            if (removes.Count > 0)
            {
                for (int i = removes.Count - 1; i >= 0; i--)
                {
                    timers.RemoveAt(removes[i]);
                }
            }
        }
    }
}
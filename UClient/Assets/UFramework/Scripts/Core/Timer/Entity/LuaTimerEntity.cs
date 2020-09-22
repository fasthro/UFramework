/*
 * @Author: fasthro
 * @Date: 2020-09-22 17:24:34
 * @Description: lua timer
 */
using LuaInterface;
using UFramework.Messenger;
using UFramework.Pool;

namespace UFramework.Timers
{
    public class LuaTimerEntity : TimerEntity
    {
        public LuaTable luaself { get; private set; }
        public LuaFunction luaFunc { get; private set; }

        #region pool

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">在计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="self">每帧更新事件</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="useRealTime">是否使用真实时间不受时间缩放影响</param>
        /// <returns></returns>
        public static LuaTimerEntity LuaAllocate(float duration, LuaFunction onComplete, LuaTable self = null, bool isLooped = false, bool useRealTime = false)
        {
            return ObjectPool<LuaTimerEntity>.Instance.Allocate().LuaBuilder(duration, onComplete, self, isLooped, useRealTime);
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            if (luaself != null)
                luaself.Dispose();
            luaself = null;

            if (luaFunc != null)
                luaFunc.Dispose();
            luaFunc = null;
        }

        public override void Recycle()
        {
            ObjectPool<LuaTimerEntity>.Instance.Recycle(this);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">在计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="self">每帧更新事件</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="useRealTime">是否使用真实时间不受时间缩放影响</param>
        /// <returns></returns>
        private LuaTimerEntity LuaBuilder(float duration, LuaFunction onComplete, LuaTable self, bool isLooped, bool usesRealTime)
        {
            Builder(duration, null, null, isLooped, usesRealTime);
            this.luaFunc = onComplete;
            this.luaself = self;
            return this;
        }

        public override void OnUpdate()
        {
            if (isIneffective) return;

            var wt = GetWorldTime();
            if (isPaused)
            {
                _startTime += GetTimeDelta();
                _lastUpdateTime = wt;
                return;
            }

            _lastUpdateTime = wt;

            if (wt >= GetFireTime())
            {
                if (luaself == null) luaFunc.Call();
                else luaFunc.Call<LuaTable>(luaself);

                if (isLooped) _startTime = wt;
                else isCompleted = true;
            }
        }
    }
}
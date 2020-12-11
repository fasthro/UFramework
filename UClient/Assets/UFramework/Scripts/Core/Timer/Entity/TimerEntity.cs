/*
 * @Author: fasthro
 * @Date: 2020-09-22 16:07:15
 * @Description: timer entity
 */
using UnityEngine;

namespace UFramework.Core
{
    public class TimerEntity : IPoolBehaviour
    {
        #region public
        /// <summary>
        /// 时常
        /// </summary>
        /// <value></value>
        public float duration { get; private set; }

        /// <summary>
        /// 是否循环
        /// </summary>
        /// <value></value>
        public bool isLooped { get; private set; }

        /// <summary>
        /// 是否已经完成
        /// </summary>
        /// <value></value>
        public bool isCompleted { get; protected set; }

        /// <summary>
        /// 是否使用真实时间不受时间缩放印象
        /// </summary>
        /// <value></value>
        public bool usesRealTime { get; private set; }

        /// <summary>
        /// 是否已暂停
        /// </summary>
        /// <value></value>
        public bool isPaused
        {
            get { return _timeElapsedBeforePause.HasValue; }
        }

        /// <summary>
        /// 是否已取消
        /// </summary>
        /// <value></value>
        public bool isCancelled
        {
            get { return _timeElapsedBeforeCancel.HasValue; }
        }

        /// <summary>
        /// 是否已失效
        /// </summary>
        /// <value></value>
        public bool isIneffective
        {
            get { return isCompleted || isCancelled || isRecycled; }
        }

        #endregion

        #region protected

        protected UCallback _onComplete;
        protected UCallback<float> _onUpdate;
        protected float _startTime;
        protected float _lastUpdateTime;

        protected float? _timeElapsedBeforeCancel;
        protected float? _timeElapsedBeforePause;

        #endregion

        #region pool

        public bool isRecycled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">在计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="onUpdate">每帧更新事件</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="useRealTime">是否使用真实时间不受时间缩放影响</param>
        /// <returns></returns>
        public static TimerEntity Allocate(float duration, UCallback onComplete, UCallback<float> onUpdate = null, bool isLooped = false, bool useRealTime = false)
        {
            return ObjectPool<TimerEntity>.Instance.Allocate().Builder(duration, onComplete, onUpdate, isLooped, useRealTime);
        }

        public virtual void OnRecycle()
        {
            _timeElapsedBeforeCancel = null;
            _timeElapsedBeforePause = null;
            _onComplete = null;
            _onUpdate = null;
            isCompleted = false;
        }

        public virtual void Recycle()
        {
            ObjectPool<TimerEntity>.Instance.Recycle(this);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">在计时器启动前等待的时间，以秒为单位。</param>
        /// <param name="onComplete">完成事件</param>
        /// <param name="onUpdate">每帧更新事件</param>
        /// <param name="isLooped">是否循环</param>
        /// <param name="useRealTime">是否使用真实时间不受时间缩放影响</param>
        /// <returns></returns>
        protected TimerEntity Builder(float duration, UCallback onComplete, UCallback<float> onUpdate, bool isLooped, bool usesRealTime)
        {
            this.duration = duration;
            this._onComplete = onComplete;
            this._onUpdate = onUpdate;
            this.isLooped = isLooped;
            this.usesRealTime = usesRealTime;
            this._startTime = this._lastUpdateTime = this.GetWorldTime();

            return this;
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            if (isIneffective) return;
            _timeElapsedBeforeCancel = GetTimeElapsed();
            _timeElapsedBeforePause = null;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (isIneffective || isPaused) return;
            _timeElapsedBeforePause = this.GetTimeElapsed();
        }

        /// <summary>
        /// 唤醒
        /// </summary>
        public void Resume()
        {
            if (isIneffective || isPaused) return;
            _timeElapsedBeforePause = null;
        }

        /// <summary>
        /// 已经过去得时间
        /// </summary>
        /// <returns>返回自计时器当前周期开始以来所经过的秒数
        /// 如果计时器是循环的，则为当前循环;如果计时器不是循环的，则为start
        /// 如果定时器已经完成运行，这等于持续时间，如果计时器被取消/暂停，这等于计时器之间传递的秒数</returns>
        public float GetTimeElapsed()
        {
            if (isCompleted || GetWorldTime() >= GetFireTime())
                return duration;

            return _timeElapsedBeforeCancel ??
                   _timeElapsedBeforePause ??
                   GetWorldTime() - _startTime;
        }

        /// <summary>
        /// 剩余时间
        /// </summary>
        /// <returns></returns>
        public float GetTimeRemaining()
        {
            return duration - GetTimeElapsed();
        }

        /// <summary>
        /// 定时器过去时间百分比(可用于进度条展示)
        /// </summary>
        /// <returns>0~1</returns>
        public float GetRatioComplete()
        {
            return GetTimeElapsed() / duration;
        }

        /// <summary>
        /// 定时器剩余时间百分比(可用于进度条展示)
        /// </summary>
        /// <returns>0~1</returns>
        public float GetRatioRemaining()
        {
            return GetTimeRemaining() / duration;
        }

        protected float GetWorldTime()
        {
            return usesRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        protected float GetFireTime()
        {
            return _startTime + duration;
        }

        protected float GetTimeDelta()
        {
            return GetWorldTime() - _lastUpdateTime;
        }

        public virtual void OnUpdate()
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

            if (_onUpdate != null)
                _onUpdate.InvokeGracefully(GetTimeElapsed());

            if (wt >= GetFireTime())
            {
                _onComplete.InvokeGracefully();
                if (isLooped) _startTime = wt;
                else isCompleted = true;
            }
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-06-15 23:25:11
 * @Description: 定时器
 */

using System;
using System.Collections.Generic;
using LuaInterface;
using UFramework.Messenger;

namespace UFramework.Timer
{
    [MonoSingletonPath("UFramework/UTimer")]
    public class UTimer : MonoSingleton<UTimer>
    {
        private float interval = 0;
        private object mlock = new object();

        private List<TimeTicker> expireTickers = new List<TimeTicker>();
        private List<TimeTicker> timeTickers = new List<TimeTicker>();

        private List<TimerInfo> expireTimers = new List<TimerInfo>();
        private HashSet<TimerInfo> timers = new HashSet<TimerInfo>();

        private List<Ticker> delTickers = new List<Ticker>();
        static HashSet<Ticker> tickers = new HashSet<Ticker>();

        /// <summary>
        /// 添加定时器
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="interval"></param>
        /// <param name="callback"></param>
        /// <param name="param"></param>
        /// <param name="runNow"></param>
        /// <returns></returns>
        private TimerInfo _Add(float expires, float interval, UCallback<object> callback, object param, bool runNow)
        {
            var timer = new TimerInfo();
            timer.interval = interval;
            timer.callback = callback;
            timer.param = param;
            timer.expire = expires;
            timer.tick = runNow ? interval : 0;
            timers.Add(timer);
            return timer;
        }

        /// <summary>
        /// 添加Lua定时器
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="interval"></param>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="param"></param>
        /// <param name="runNow"></param>
        /// <returns></returns>
        private TimerInfo _AddLua(float expires, float interval, LuaTable self, LuaFunction func, object param, bool runNow)
        {
            var timer = new TimerInfo();
            timer.interval = interval;
            timer.luaself = self;
            timer.luaFunc = func;
            timer.param = param;
            timer.expire = expires;
            timer.tick = runNow ? interval : 0;
            timers.Add(timer);
            return timer;
        }

        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timer"></param>
        private void _Remove(TimerInfo timer)
        {
            if (timer != null)
            {
                expireTimers.Add(timer);
            }
        }

        /// <summary>
        /// 定时器轮训
        /// </summary>
        /// <param name="deltaTime"></param>
        private void OnTimer(float deltaTime)
        {
            if (timers.Count == 0)
            {
                return;
            }
            foreach (var timer in timers)
            {
                if (expireTimers.Contains(timer))
                {
                    continue;
                }
                timer.tick += deltaTime;
                if (timer.expire > 0)
                {
                    if (timer.tick >= timer.expire)
                    {
                        expireTimers.Add(timer);
                        if (timer.luaFunc != null)
                        {
                            if (timer.luaself == null)
                            {
                                timer.luaFunc.Call<object>(timer.param);
                            }
                            else
                            {
                                timer.luaFunc.Call<LuaTable, object>(timer.luaself, timer.param);
                            }
                        }
                        if (timer.callback != null)
                        {
                            timer.callback.Invoke(timer.param);
                        }
                    }
                }
                else
                {
                    if (timer.tick >= timer.interval)
                    {
                        timer.tick = 0;
                        if (timer.luaFunc != null)
                        {
                            timer.luaFunc.Call<LuaTable, object>(timer.luaself, timer.param);
                        }
                        if (timer.callback != null)
                        {
                            timer.callback.Invoke(timer.param);
                        }
                    }
                }
            }
            lock (mlock)
            {
                foreach (var timer in expireTimers)
                {
                    if (timer.luaself != null)
                    {
                        timer.luaself.Dispose();
                        timer.luaself = null;
                    }
                    if (timer.luaFunc != null)
                    {
                        timer.luaFunc.Dispose();
                        timer.luaFunc = null;
                    }
                    timers.Remove(timer);
                }
                expireTimers.Clear();
            }
        }

        /// <summary>
        /// 添加帧事件
        /// </summary>
        /// <param name="kv"></param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        private void _AddFrameCallbacks(Dictionary<uint, uint> kv, object param, UCallback<uint, object> callback)
        {
            foreach (var de in kv)
            {
                var ticker = new TimeTicker();
                ticker.typeId = de.Key;
                ticker.frameCount = de.Value;
                ticker.refCount = 0;
                ticker.callback = callback;
                ticker.param = param;
                timeTickers.Add(ticker);
            }
        }

        /// <summary>
        /// 帧事件轮训
        /// </summary>
        /// <param name="deltaTime"></param>
        private void OnTimeTicker(float deltaTime)
        {
            if (timeTickers.Count == 0)
            {
                return;
            }
            foreach (TimeTicker ticker in timeTickers)
            {
                if (ticker.refCount == ticker.frameCount)
                {
                    expireTickers.Add(ticker);
                    ticker.callback(ticker.typeId, ticker.param);
                }
                else
                {
                    ticker.refCount++;
                }
            }
            foreach (var timer in expireTickers)
            {
                timeTickers.Remove(timer);
            }
            expireTickers.Clear();
        }

        /// <summary>
        /// 添加 Ticker
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private Ticker _AddTicker(uint ms, object param, UCallback<object> callback)
        {
            var ticker = new Ticker();
            ticker.param = param;
            ticker.callback = callback;
            ticker.frameCount = ticker.remain = TimeToFrame(ms);
            tickers.Add(ticker);
            return ticker;
        }

        /// <summary>
        /// Ticker 轮训
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnTicker(float deltaTime)
        {
            if (tickers.Count > 0)
            {
                delTickers.Clear();
                foreach (Ticker ticker in tickers)
                {
                    if (ticker.frameCount > 0)
                    {
                        if (--ticker.remain == 0)
                        {
                            delTickers.Add(ticker);
                        }
                    }
                    if (ticker.callback != null)
                    {
                        ticker.callback(ticker.param);
                    }
                }
                foreach (var t in delTickers)
                {
                    tickers.Remove(t);
                }
            }
        }

        public static uint TimeToFrame(uint ms)
        {
            return ms / 1000 * 33;
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            OnTimer(deltaTime);
            OnTimeTicker(deltaTime);
            OnTicker(deltaTime);
        }

        #region API

        /// <summary>
        /// 添加定时器
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="interval"></param>
        /// <param name="callback">回调</param>
        /// <param name="param">附加参数</param>
        /// <param name="runNow">是否立即运行</param>
        /// <returns></returns>
        public static TimerInfo Add(float expires, float interval, UCallback<object> callback, object param = null, bool runNow = false)
        {
            return Instance._Add(expires, interval, callback, param, runNow);
        }

        /// <summary>
        /// 添加 Lua 定时器
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="interval"></param>
        /// <param name="self"></param>
        /// <param name="func"></param>
        /// <param name="param"></param>
        /// <param name="runNow"></param>
        /// <returns></returns>
        public static TimerInfo AddLua(float expires, float interval, LuaTable self, LuaFunction func, object param, bool runNow)
        {
            return Instance._AddLua(expires, interval, self, func, param, runNow);
        }

        /// <summary>
        /// 移除定时器
        /// </summary>
        /// <param name="timer"></param>
        public static void Remove(TimerInfo timer)
        {
            Instance._Remove(timer);
        }

        /// <summary>
        /// 添加帧事件
        /// </summary>
        /// <param name="kv"></param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        public static void AddFrameCallbacks(Dictionary<uint, uint> kv, object param, UCallback<uint, object> callback)
        {
            Instance._AddFrameCallbacks(kv, param, callback);
        }

        /// <summary>
        /// 添加 Ticker
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="param"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static Ticker AddTicker(uint ms, object param, UCallback<object> callback)
        {
            return Instance._AddTicker(ms, param, callback);
        }

        #endregion
    }
}
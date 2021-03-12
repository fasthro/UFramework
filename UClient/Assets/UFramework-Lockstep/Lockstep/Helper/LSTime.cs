// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-12-15 16:55:54
// * @Description:
// --------------------------------------------------------------------------------

using System;

namespace Lockstep
{
    public static class LSTime
    {
        /// <summary>
        /// The total number of frames that have passed (Read Only).
        /// </summary>
        /// <value></value>
        public static int frameCount { get; private set; }

        /// <summary>
        /// The completion time in seconds since the last frame (Read Only).
        /// </summary>
        /// <value></value>
        public static FP deltaTime { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static long deltaTimeMS { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public static FP tickDeltaTime { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public static FP tickDeltaTimeMS { get; private set; }

        /// <summary>
        /// The time this frame has started (Read Only). This is the time in seconds since the last
        /// level has been loaded.
        /// </summary>
        /// <value></value>
        public static FP timeSinceLevelLoad { get; private set; }

        /// <summary>
        /// The real time in seconds since the game started (Read Only).
        /// </summary>
        /// <value></value>
        public static float realtimeSinceStartup => (float) (DateTime.Now - _initTime).TotalSeconds;

        public static long realtimeSinceStartupMS => (long) (DateTime.Now - _initTime).TotalMilliseconds;

        public static void Initialize()
        {
            _initTime = DateTime.Now;
            _lastFrameTime = DateTime.Now;
            tickDeltaTimeMS = 1000 / LSDefine.FRAME_RATE;
            tickDeltaTime = tickDeltaTimeMS / 1000;
        }

        public static void Update()
        {
            var now = DateTime.Now;

            deltaTime = (FP) (now - _lastFrameTime).TotalSeconds;
            deltaTimeMS = (long) (now - _lastFrameTime).TotalMilliseconds;

            timeSinceLevelLoad = (FP) (now - _initTime).TotalSeconds;
            frameCount++;
            _lastFrameTime = now;
        }

        public new static string ToString()
        {
            return $"LSTime > frameCount: {frameCount}, deltaTime: {deltaTime}, timeSinceLevelLoad: {timeSinceLevelLoad}, realtimeSinceStartup: {realtimeSinceStartup}";
        }

        private static DateTime _initTime;
        private static DateTime _lastFrameTime;
    }
}
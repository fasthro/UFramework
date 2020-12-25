/*
 * @Author: fasthro
 * @Date: 2020-12-15 16:55:54
 * @Description: Time
 */

using System;

namespace LSC
{
    public class LSTime
    {
        private static DateTime _initTime;

        private static DateTime _lastFrameTime;

        /// <summary>
        /// The total number of frames that have passed (Read Only).
        /// </summary>
        /// <value></value>
        public static int frameCount { get; private set; }

        /// <summary>
        /// The completion time in seconds since the last frame (Read Only).
        /// </summary>
        /// <value></value>
        public static float deltaTime { get; private set; }

        public static long deltaTimeMS { get; private set; }

        /// <summary>
        /// The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded.
        /// </summary>
        /// <value></value>
        public static float timeSinceLevelLoad { get; private set; }

        /// <summary>
        /// The real time in seconds since the game started (Read Only).
        /// </summary>
        /// <value></value>
        public static float realtimeSinceStartup => (float)((DateTime.Now - _initTime).TotalSeconds);

        public static long realtimeSinceStartupMS => (long)((DateTime.Now - _initTime).TotalMilliseconds);

        public static void Initialize()
        {
            _initTime = DateTime.Now;
            _lastFrameTime = DateTime.Now;
        }

        public static void Update(float dt)
        {
            var now = DateTime.Now;

            deltaTime = (float)((now - _lastFrameTime).TotalSeconds);
            deltaTimeMS = (long)((now - _lastFrameTime).TotalMilliseconds);

            timeSinceLevelLoad = (float)((now - _initTime).TotalSeconds);
            frameCount++;
            _lastFrameTime = now;
        }

        new public static string ToString()
        {
            return $"LSTime > frameCount: {frameCount}, deltaTime: {deltaTime}, timeSinceLevelLoad: {timeSinceLevelLoad}, realtimeSinceStartup: {realtimeSinceStartup}";
        }
    }
}
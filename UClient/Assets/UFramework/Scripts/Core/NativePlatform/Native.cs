/*
 * @Author: fasthro
 * @Date: 2020-07-30 16:52:01
 * @Description: 原生平台
 */

namespace UFramework.NativePlatform
{
    [MonoSingletonPath("UFramework/Native")]
    public class Native : MonoSingleton<Native>
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        /// <returns></returns>
        public static NativeDevice Device { get; private set; }

        /// <summary>
        /// utils
        /// </summary>
        /// <returns></returns>
        public static NativeUtils Utils { get; private set; }

        /// <summary>
        /// awake
        /// </summary>
        protected override void OnSingletonAwake()
        {
            Device = new NativeDevice();
            Utils = new NativeUtils();

#if !UNITY_EDITOR && UNITY_ANDROID
            AndroidNative.Initialize();
#endif
        }
    }
}
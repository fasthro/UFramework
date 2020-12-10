/*
 * @Author: fasthro
 * @Date: 2020-07-30 16:52:01
 * @Description: Native
 */

namespace UFramework.Natives
{
    [MonoSingletonPath("UFramework/Native")]
    public class Native : MonoSingleton<Native>
    {

        /// <summary>
        /// 设备信息
        /// </summary>
        /// <returns></returns>
        public static Device Device { get; private set; }

        /// <summary>
        /// utils
        /// </summary>
        /// <returns></returns>
        public static Utils Util { get; private set; }

        /// <summary>
        /// awake
        /// </summary>
        protected override void OnSingletonAwake()
        {
            Device = new Device();
            Util = new Utils();

#if !UNITY_EDITOR && UNITY_ANDROID
            NativeAndroid.Initialize();
#endif
        }
    }
}
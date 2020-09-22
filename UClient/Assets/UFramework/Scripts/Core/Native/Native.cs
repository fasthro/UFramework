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
        public static Device device { get; private set; }

        /// <summary>
        /// utils
        /// </summary>
        /// <returns></returns>
        public static Utils utils { get; private set; }

        /// <summary>
        /// awake
        /// </summary>
        protected override void OnSingletonAwake()
        {
            device = new Device();
            utils = new Utils();

#if !UNITY_EDITOR && UNITY_ANDROID
            NativeAndroid.Initialize();
#endif
        }
    }
}
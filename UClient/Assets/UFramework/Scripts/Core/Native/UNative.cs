/*
 * @Author: fasthro
 * @Date: 2020-07-30 16:52:01
 * @Description: Native
 */
using UFramework.Native.Service;

namespace UFramework.Native
{
    [MonoSingletonPath("UFramework/UNative")]
    public class UNative : MonoSingleton<UNative>
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
        public static Service.Utils utils { get; private set; }

        /// <summary>
        /// awake
        /// </summary>
        protected override void OnSingletonAwake()
        {
            device = new Device();
            utils = new Service.Utils();

#if !UNITY_EDITOR && UNITY_ANDROID
            NativeAndroid.Initialize();
#endif
        }
    }
}
/*
 * @Author: fasthro
 * @Date: 2020-07-30 16:58:45
 * @Description: device
 */
namespace UFramework.Native.Service
{
    public interface IDevice
    {
        /// <summary>
        /// 设备唯一标识
        /// </summary>
        /// <returns></returns>
        string DeviceId();

        /// <summary>
        /// 设备所处国家ISO
        /// </summary>
        /// <returns></returns>
        string DeviceCountryISO();
    }
    
    public class DeviceAndroid : IDevice
    {
        public string DeviceId()
        {
            return NativeAndroid.AppNative.CallStatic<string>("getDeviceId");
        }

        public string DeviceCountryISO()
        {
            return NativeAndroid.AppNative.CallStatic<string>("getDeviceCountry");
        }
    }

    public class DeviceIOS : IDevice
    {
        public string DeviceId()
        {
            throw new System.NotImplementedException();
        }

        public string DeviceCountryISO()
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// 设备服务
    /// </summary>
    public class Device : IDevice
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        static DeviceAndroid android = new DeviceAndroid();
#elif !UNITY_EDITOR && UNITY_IOS
        static DeviceIOS ios = new DeviceIOS();
#endif
        /// <summary>
        /// 设备唯一标识
        /// </summary>
        /// <returns></returns>
        public string DeviceId()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return android.DeviceId();
#elif !UNITY_EDITOR && UNITY_IOS
            return ios.DeviceId();
#else
            return null;
#endif
        }

        /// <summary>
        /// 设备所处国家
        /// </summary>
        /// <returns></returns>
        public string DeviceCountryISO()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return android.DeviceCountryISO();
#elif !UNITY_EDITOR && UNITY_IOS
            return ios.DeviceCountryISO();
#else
            return null;
#endif
        }
    }
}
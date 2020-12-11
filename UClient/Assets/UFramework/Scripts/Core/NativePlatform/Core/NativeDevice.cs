/*
 * @Author: fasthro
 * @Date: 2020-07-30 16:58:45
 * @Description: device
 */
namespace UFramework.NativePlatform
{
    public interface INativeDevice
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

    public class AndroidDevice : INativeDevice
    {
        public string DeviceId()
        {
            return AndroidNative.NativeClass.CallStatic<string>("getDeviceId");
        }

        public string DeviceCountryISO()
        {
            return AndroidNative.NativeClass.CallStatic<string>("getDeviceCountry");
        }
    }

    public class IOSDevice : INativeDevice
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

    public class UnknownDevice : INativeDevice
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
    public class NativeDevice : INativeDevice
    {
        private UnknownDevice _unknown = new UnknownDevice();
        private AndroidDevice _android = new AndroidDevice();
        private IOSDevice _ios = new IOSDevice();

        /// <summary>
        /// 设备唯一标识
        /// </summary>
        /// <returns></returns>
        public string DeviceId()
        {
            return GetChannel().DeviceId();
        }

        /// <summary>
        /// 设备所处国家
        /// </summary>
        /// <returns></returns>
        public string DeviceCountryISO()
        {
            return GetChannel().DeviceCountryISO();
        }

        INativeDevice GetChannel()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return _android;
#elif !UNITY_EDITOR && UNITY_IOS
            return _ios;
#else
            return _unknown;
#endif
        }
    }
}
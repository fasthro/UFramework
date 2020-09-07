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

    public class AndroidDevice : IDevice
    {
        public string DeviceId()
        {
            return NativeAndroid.native.CallStatic<string>("getDeviceId");
        }

        public string DeviceCountryISO()
        {
            return NativeAndroid.native.CallStatic<string>("getDeviceCountry");
        }
    }

    public class IOSDevice : IDevice
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

    public class UnknownDevice : IDevice
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
        private UnknownDevice unknown = new UnknownDevice();
        private AndroidDevice android = new AndroidDevice();
        private IOSDevice ios = new IOSDevice();

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

        IDevice GetChannel()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return android;
#elif !UNITY_EDITOR && UNITY_IOS
            return ios;
#else
            return unknown;
#endif
        }
    }
}
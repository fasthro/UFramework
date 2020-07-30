/*
 * @Author: fasthro
 * @Date: 2020-07-30 17:03:25
 * @Description: interface device
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
}
/*
 * @Author: fasthro
 * @Date: 2020-07-30 17:30:14
 * @Description: GooglePalyService Obb
 */
namespace UFramework.Native.Service
{
    public class GooglePlayServiceObb
    {
        public void Check()
        {
            NativeAndroid.mainNative.CallStatic("checkGPSObb", 162);
        }
    }
}
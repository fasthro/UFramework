/*
 * @Author: fasthro
 * @Date: 2020-07-30 17:31:42
 * @Description: GooglePalyService
 */
namespace UFramework.Native.Service
{
    public class GooglePlayService
    {
        /// <summary>
        /// obb
        /// </summary>
        /// <returns></returns>
        public GooglePlayServiceObb obb = new GooglePlayServiceObb();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwINnSEZSgRJL+ASiU/iF/a2RSC/hzDG/PB2bQrxFWN0BURjhZH2UV6ysT7sjIv57CHulOq31VHyBjE5kQyivhcK2sOiHByyH8z3aLn2lxnAXAA38Cb0QbL3PV6Gifl29lyxtrGcAlO/fI5GxqLvwT312g/SeOncXroLQwVxFW/EPWjz9PrJ6R4BL5ivs1Us2/NW1eWh89d+aWp0LSbBXq0YUoiqV5Nju1k9toqOYH1Ztxp308RmhNx31iPowiwOlTU0WRUZzVifmz27BbjSW/ufWPYPWhBKafQCDIt5UdkZ82zsbkrf7/T8k+FSJxLMJBdq7muruXvF/QXBh8MpHkwIDAQAB");
        }


        /// <summary>
        /// 设置 Public Key
        /// </summary>
        /// <param name="publicKey"></param>
        public void SetPublicKey(string publicKey)
        {
            NativeAndroid.mainNative.CallStatic("setGPSPublicKey", publicKey);
        }
    }
}
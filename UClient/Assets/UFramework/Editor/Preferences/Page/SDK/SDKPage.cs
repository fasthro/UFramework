/*
 * @Author: fasthro
 * @Date: 2020-07-23 00:49:46
 * @Description: sdk preferences
 */
namespace UFramework.Editor.Preferences
{
    public class SDKPage : IPage
    {
        public string menuName { get { return "SDK"; } }

        public object GetInstance()
        {
            return this;
        }

        public void OnDrawFunctoinButton()
        {
        }

        public void OnRenderBefore()
        {

        }

        public void OnApply()
        {
        }
    }
}
// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-28 14:01:30
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Editor.Preferences
{
    public class WelcomePage : IPage, IPageBar
    {
        public string menuName => "Welcome";

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
        }

        public void OnPageBarDraw()
        {

        }

        public void OnSaveDescribe()
        {
            
        }
    }
}
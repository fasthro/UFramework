/*
 * @Author: fasthro
 * @Date: 2020-08-08 20:03:11
 * @Description: custom wrap setting
 */
namespace UFramework.Editor.Preferences
{
    public class WrapTypeObject
    {
        
    }

    public class WrapPage : IPage, IPageBar
    {
        public string menuName { get { return "WrapSetting"; } }

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
    }
}
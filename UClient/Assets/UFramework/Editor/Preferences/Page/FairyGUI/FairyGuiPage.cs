/*
 * @Author: fasthro
 * @Date: 2020-09-15 15:49:23
 * @Description: FairyGUI preferences
 */
namespace UFramework.Editor.Preferences
{
    public class FairyGUIPage : IPage, IPageBar
    {
        public string menuName { get { return "FairyGUI"; } }

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
        }

        public void OnSaveDescribe()
        {
        }

        public void OnPageBarDraw()
        {
        }

    }
}
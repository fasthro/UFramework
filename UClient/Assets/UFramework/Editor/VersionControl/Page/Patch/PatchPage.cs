/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:50
 * @Description: patch
 */

namespace UFramework.Editor.VersionControl
{
    public class PatchPage : IPage, IPageBar
    {
        public string menuName { get { return "Patch"; } }

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
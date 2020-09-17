/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: build
 */

namespace UFramework.Editor.VersionControl
{
    public class BuilderPage : IPage, IPageBar
    {
        public string menuName { get { return "Build"; } }

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
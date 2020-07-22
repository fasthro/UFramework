/*
 * @Author: fasthro
 * @Date: 2020-07-23 00:38:10
 * @Description: svn page
 */
namespace UFramework.Editor.Tool
{
    public class SvnPage : IPage
    {
        public string menuName { get { return "SVN"; } }

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
    }
}
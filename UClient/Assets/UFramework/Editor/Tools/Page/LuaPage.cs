/*
 * @Author: fasthro
 * @Date: 2020-07-19 00:15:10
 * @Description: lua tools page
 */
namespace UFramework.Editor.Tool
{
    public class LuaPage : IPage
    {
        public string menuName { get { return "Lua"; } }

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
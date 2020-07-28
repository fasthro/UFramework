/*
 * @Author: fasthro
 * @Date: 2020-07-19 00:15:10
 * @Description: lua tools page
 */
using Sirenix.OdinInspector;

namespace UFramework.Editor.Tool
{
    public class LuaPage : IPage
    {
        public string menuName { get { return "Lua"; } }

        [FoldoutGroup("wrap")]
        [HorizontalGroup("wrap-layout")]
        [Button]
        public void GenLuaWrap()
        {

        }

        [FoldoutGroup("wrap")]
        [HorizontalGroup("wrap-layout")]
        [Button]
        public void GenDelegates()
        {

        }

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
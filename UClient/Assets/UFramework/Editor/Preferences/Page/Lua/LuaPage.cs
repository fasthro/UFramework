/*
 * @Author: fasthro
 * @Date: 2020-08-08 19:39:10
 * @Description: Lua/ToLua Page
 */
namespace UFramework.Editor.Preferences
{
    public class LuaPage : IPage, IPageBar
    {
        public string menuName { get { return "Lua"; } }

        public static string saveDir = "";//Application.dataPath + "/Source/Generate/";
        public static string toluaBaseType = "";//Application.dataPath + "/ToLua/BaseType/";
        public static string baseLuaDir = "";//Application.dataPath + "/ToLua/Lua/";
        public static string injectionFilesPath = "";//Application.dataPath + "/ToLua/Injection/";

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
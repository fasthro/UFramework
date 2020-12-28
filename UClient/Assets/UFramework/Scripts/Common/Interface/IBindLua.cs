/*
 * @Author: fasthro
 * @Date: 2020-12-28 11:30:41
 * @Description: 
 */
using LuaInterface;

namespace UFramework
{
    public interface IBindLua
    {
        LuaTable luaTable { get; set; }

        [NoToLua]
        bool CallLuaFunction(string funcName, params object[] args);
    }
}
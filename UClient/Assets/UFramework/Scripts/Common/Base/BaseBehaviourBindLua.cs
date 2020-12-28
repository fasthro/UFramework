/*
 * @Author: fasthro
 * @Date: 2020-12-28 11:02:48
 * @Description: 
 */
using System;
using LuaInterface;

namespace UFramework
{
    public abstract class BaseBehaviourBindLua : BaseBehaviour, IBindLua
    {
        public LuaTable luaTable { get; set; }

        public bool CallLuaFunction(string funcName, params object[] args)
        {
            if (luaTable != null)
            {
                LuaFunction ctor = luaTable.GetLuaFunction(funcName);
                if (ctor != null)
                {
                    try
                    {
                        if (args.Length == 0)
                            ctor.Call(luaTable);
                        else if (args.Length == 1)
                            ctor.Call(luaTable, args[0]);
                        else if (args.Length == 2)
                            ctor.Call(luaTable, args[0], args[1]);
                        else if (args.Length == 3)
                            ctor.Call(luaTable, args[0], args[1], args[2]);
                    }
                    catch (Exception err)
                    {
                        Logger.Error(err);
                    }
                    ctor.Dispose();
                    return true;
                }
            }
            return false;
        }

        new public void Dispose()
        {
            luaTable?.Dispose();
            luaTable = null;

            OnDispose();
        }
    }
}
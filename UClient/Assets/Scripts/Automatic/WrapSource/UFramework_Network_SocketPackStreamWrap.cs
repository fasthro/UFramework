﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_Network_SocketPackStreamWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UFramework.Network.SocketPackStream), typeof(UFramework.Network.SocketPackLine));
		L.RegFunction("GetString", new LuaCSFunction(GetString));
		L.RegFunction("New", new LuaCSFunction(_CreateUFramework_Network_SocketPackStream));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUFramework_Network_SocketPackStream(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UFramework.Network.SocketPackStream obj = new UFramework.Network.SocketPackStream();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1)
			{
				byte[] arg0 = ToLua.CheckByteBuffer(L, 1);
				UFramework.Network.SocketPackStream obj = new UFramework.Network.SocketPackStream(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UFramework.Network.SocketPackStream.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetString(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UFramework.Network.SocketPackStream obj = (UFramework.Network.SocketPackStream)ToLua.CheckObject<UFramework.Network.SocketPackStream>(L, 1);
			string o = obj.GetString();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}


﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_Network_SocketPackRawByteWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UFramework.Network.SocketPackRawByte), typeof(UFramework.Network.SocketPackLinear));
		L.RegFunction("ParseString", new LuaCSFunction(ParseString));
		L.RegFunction("New", new LuaCSFunction(_CreateUFramework_Network_SocketPackRawByte));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUFramework_Network_SocketPackRawByte(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UFramework.Network.SocketPackRawByte obj = new UFramework.Network.SocketPackRawByte();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 1)
			{
				byte[] arg0 = ToLua.CheckByteBuffer(L, 1);
				UFramework.Network.SocketPackRawByte obj = new UFramework.Network.SocketPackRawByte(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UFramework.Network.SocketPackRawByte.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ParseString(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				UFramework.Network.SocketPackRawByte obj = (UFramework.Network.SocketPackRawByte)ToLua.CheckObject<UFramework.Network.SocketPackRawByte>(L, 1);
				string o = obj.ParseString();
				LuaDLL.lua_pushstring(L, o);
				return 1;
			}
			else if (count == 2)
			{
				UFramework.Network.SocketPackRawByte obj = (UFramework.Network.SocketPackRawByte)ToLua.CheckObject<UFramework.Network.SocketPackRawByte>(L, 1);
				System.Text.Encoding arg0 = (System.Text.Encoding)ToLua.CheckObject<System.Text.Encoding>(L, 2);
				string o = obj.ParseString(arg0);
				LuaDLL.lua_pushstring(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UFramework.Network.SocketPackRawByte.ParseString");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}


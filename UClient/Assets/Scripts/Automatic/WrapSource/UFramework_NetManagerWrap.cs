﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_NetManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UFramework.NetManager), typeof(UFramework.BaseManager));
		L.RegFunction("OnAwake", new LuaCSFunction(OnAwake));
		L.RegFunction("Connecte", new LuaCSFunction(Connecte));
		L.RegFunction("Redirect", new LuaCSFunction(Redirect));
		L.RegFunction("Disconnecte", new LuaCSFunction(Disconnecte));
		L.RegFunction("Send", new LuaCSFunction(Send));
		L.RegFunction("OnSocketConnected", new LuaCSFunction(OnSocketConnected));
		L.RegFunction("OnSocketDisconnected", new LuaCSFunction(OnSocketDisconnected));
		L.RegFunction("OnSocketReceive", new LuaCSFunction(OnSocketReceive));
		L.RegFunction("OnSocketException", new LuaCSFunction(OnSocketException));
		L.RegFunction("OnUpdate", new LuaCSFunction(OnUpdate));
		L.RegFunction("OnDestroy", new LuaCSFunction(OnDestroy));
		L.RegFunction("New", new LuaCSFunction(_CreateUFramework_NetManager));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("isConnected", new LuaCSFunction(get_isConnected), null);
		L.RegVar("isRedirecting", new LuaCSFunction(get_isRedirecting), null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUFramework_NetManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UFramework.NetManager obj = new UFramework.NetManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UFramework.NetManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnAwake(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			obj.OnAwake();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Connecte(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
			obj.Connecte(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Redirect(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
			obj.Redirect(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Disconnecte(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			obj.Disconnecte();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Send(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			UFramework.Core.SocketPack arg0 = (UFramework.Core.SocketPack)ToLua.CheckObject<UFramework.Core.SocketPack>(L, 2);
			obj.Send(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSocketConnected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			obj.OnSocketConnected();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSocketDisconnected(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			obj.OnSocketDisconnected();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSocketReceive(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			UFramework.Core.SocketPack arg0 = (UFramework.Core.SocketPack)ToLua.CheckObject<UFramework.Core.SocketPack>(L, 2);
			obj.OnSocketReceive(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnSocketException(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			System.Exception arg0 = (System.Exception)ToLua.CheckObject<System.Exception>(L, 2);
			obj.OnSocketException(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdate(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.OnUpdate(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDestroy(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)ToLua.CheckObject<UFramework.NetManager>(L, 1);
			obj.OnDestroy();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isConnected(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)o;
			bool ret = obj.isConnected;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isConnected on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isRedirecting(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UFramework.NetManager obj = (UFramework.NetManager)o;
			bool ret = obj.isRedirecting;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isRedirecting on a nil value");
		}
	}
}


﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_ManagerServiceWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UFramework.ManagerService), typeof(UFramework.BaseService));
		L.RegFunction("RegisterManager", new LuaCSFunction(RegisterManager));
		L.RegFunction("GetManager", new LuaCSFunction(GetManager));
		L.RegFunction("New", new LuaCSFunction(_CreateUFramework_ManagerService));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("container", new LuaCSFunction(get_container), new LuaCSFunction(set_container));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUFramework_ManagerService(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UFramework.ManagerService obj = new UFramework.ManagerService();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UFramework.ManagerService.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisterManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				UFramework.ManagerService obj = (UFramework.ManagerService)ToLua.CheckObject<UFramework.ManagerService>(L, 1);
				UFramework.BaseManager arg0 = (UFramework.BaseManager)ToLua.CheckObject<UFramework.BaseManager>(L, 2);
				obj.RegisterManager(arg0);
				return 0;
			}
			else if (count == 3)
			{
				UFramework.ManagerService obj = (UFramework.ManagerService)ToLua.CheckObject<UFramework.ManagerService>(L, 1);
				UFramework.BaseManager arg0 = (UFramework.BaseManager)ToLua.CheckObject<UFramework.BaseManager>(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.RegisterManager(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UFramework.ManagerService.RegisterManager");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetManager(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UFramework.ManagerService obj = (UFramework.ManagerService)ToLua.CheckObject<UFramework.ManagerService>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			UFramework.BaseManager o = obj.GetManager(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_container(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UFramework.ManagerService obj = (UFramework.ManagerService)o;
			UFramework.ManagerContainer ret = obj.container;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index container on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_container(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UFramework.ManagerService obj = (UFramework.ManagerService)o;
			UFramework.ManagerContainer arg0 = (UFramework.ManagerContainer)ToLua.CheckObject<UFramework.ManagerContainer>(L, 2);
			obj.container = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index container on a nil value");
		}
	}
}


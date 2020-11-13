﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_AppWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UFramework.App), typeof(System.Object));
		L.RegFunction("GetManager", new LuaCSFunction(GetManager));
		L.RegFunction("Initialize", new LuaCSFunction(Initialize));
		L.RegFunction("Update", new LuaCSFunction(Update));
		L.RegFunction("LateUpdate", new LuaCSFunction(LateUpdate));
		L.RegFunction("FixedUpdate", new LuaCSFunction(FixedUpdate));
		L.RegFunction("Destory", new LuaCSFunction(Destory));
		L.RegFunction("FindLuaFile", new LuaCSFunction(FindLuaFile));
		L.RegFunction("New", new LuaCSFunction(_CreateUFramework_App));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("AssetsDirectory", new LuaCSFunction(get_AssetsDirectory), null);
		L.RegVar("BuildDirectory", new LuaCSFunction(get_BuildDirectory), null);
		L.RegVar("TempDirectory", new LuaCSFunction(get_TempDirectory), null);
		L.RegVar("Version", new LuaCSFunction(get_Version), null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUFramework_App(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UFramework.App obj = new UFramework.App();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UFramework.App.New");
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
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			UFramework.BaseManager o = UFramework.App.GetManager(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Initialize(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UFramework.App.Initialize();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 1);
			UFramework.App.Update(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LateUpdate(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UFramework.App.LateUpdate();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FixedUpdate(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UFramework.App.FixedUpdate();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Destory(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			UFramework.App.Destory();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindLuaFile(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = UFramework.App.FindLuaFile(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AssetsDirectory(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, UFramework.App.AssetsDirectory);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BuildDirectory(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, UFramework.App.BuildDirectory);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TempDirectory(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, UFramework.App.TempDirectory);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Version(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushstring(L, UFramework.App.Version);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}


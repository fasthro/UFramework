﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_ResManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UFramework.ResManager), typeof(UFramework.BaseManager));
		L.RegFunction("LoadAssetAsync", new LuaCSFunction(LoadAssetAsync));
		L.RegFunction("LoadAsset", new LuaCSFunction(LoadAsset));
		L.RegFunction("LoadResourceAssetAsync", new LuaCSFunction(LoadResourceAssetAsync));
		L.RegFunction("LoadResourceAsset", new LuaCSFunction(LoadResourceAsset));
		L.RegFunction("LoadWebAsset", new LuaCSFunction(LoadWebAsset));
		L.RegFunction("UnloadAsset", new LuaCSFunction(UnloadAsset));
		L.RegFunction("New", new LuaCSFunction(_CreateUFramework_ResManager));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUFramework_ResManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				UFramework.ResManager obj = new UFramework.ResManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: UFramework.ResManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAssetAsync(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			UFramework.ResManager obj = (UFramework.ResManager)ToLua.CheckObject<UFramework.ResManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Type arg1 = ToLua.CheckMonoType(L, 3);
			UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest> arg2 = (UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest>)ToLua.CheckDelegate<UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest>>(L, 4);
			UFramework.Assets.AssetRequest o = obj.LoadAssetAsync(arg0, arg1, arg2);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UFramework.ResManager obj = (UFramework.ResManager)ToLua.CheckObject<UFramework.ResManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Type arg1 = ToLua.CheckMonoType(L, 3);
			UFramework.Assets.AssetRequest o = obj.LoadAsset(arg0, arg1);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadResourceAssetAsync(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			UFramework.ResManager obj = (UFramework.ResManager)ToLua.CheckObject<UFramework.ResManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Type arg1 = ToLua.CheckMonoType(L, 3);
			UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest> arg2 = (UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest>)ToLua.CheckDelegate<UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest>>(L, 4);
			UFramework.Assets.AssetRequest o = obj.LoadResourceAssetAsync(arg0, arg1, arg2);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadResourceAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			UFramework.ResManager obj = (UFramework.ResManager)ToLua.CheckObject<UFramework.ResManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Type arg1 = ToLua.CheckMonoType(L, 3);
			UFramework.Assets.AssetRequest o = obj.LoadResourceAsset(arg0, arg1);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadWebAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			UFramework.ResManager obj = (UFramework.ResManager)ToLua.CheckObject<UFramework.ResManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			System.Type arg1 = ToLua.CheckMonoType(L, 3);
			UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest> arg2 = (UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest>)ToLua.CheckDelegate<UFramework.Messenger.UCallback<UFramework.Assets.AssetRequest>>(L, 4);
			UFramework.Assets.AssetRequest o = obj.LoadWebAsset(arg0, arg1, arg2);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAsset(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UFramework.ResManager obj = (UFramework.ResManager)ToLua.CheckObject<UFramework.ResManager>(L, 1);
			UFramework.Assets.AssetRequest arg0 = (UFramework.Assets.AssetRequest)ToLua.CheckObject<UFramework.Assets.AssetRequest>(L, 2);
			obj.UnloadAsset(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

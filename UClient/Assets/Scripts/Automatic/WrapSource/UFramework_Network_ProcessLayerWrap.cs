﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UFramework_Network_ProcessLayerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(UFramework.Network.ProcessLayer));
		L.RegVar("All", new LuaCSFunction(get_All), null);
		L.RegVar("Lua", new LuaCSFunction(get_Lua), null);
		L.RegVar("CSharp", new LuaCSFunction(get_CSharp), null);
		L.RegFunction("IntToEnum", new LuaCSFunction(IntToEnum));
		L.EndEnum();
		TypeTraits<UFramework.Network.ProcessLayer>.Check = CheckType;
		StackTraits<UFramework.Network.ProcessLayer>.Push = Push;
	}

	static void Push(IntPtr L, UFramework.Network.ProcessLayer arg)
	{
		ToLua.Push(L, arg);
	}

	static Type TypeOf_UFramework_Network_ProcessLayer = typeof(UFramework.Network.ProcessLayer);

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(TypeOf_UFramework_Network_ProcessLayer, L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_All(IntPtr L)
	{
		ToLua.Push(L, UFramework.Network.ProcessLayer.All);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Lua(IntPtr L)
	{
		ToLua.Push(L, UFramework.Network.ProcessLayer.Lua);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CSharp(IntPtr L)
	{
		ToLua.Push(L, UFramework.Network.ProcessLayer.CSharp);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tointeger(L, 1);
		UFramework.Network.ProcessLayer o = (UFramework.Network.ProcessLayer)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

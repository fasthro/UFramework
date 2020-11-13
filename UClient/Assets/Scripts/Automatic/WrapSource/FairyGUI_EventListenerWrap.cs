﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_EventListenerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.EventListener), typeof(System.Object));
		L.RegFunction("AddCapture", new LuaCSFunction(AddCapture));
		L.RegFunction("RemoveCapture", new LuaCSFunction(RemoveCapture));
		L.RegFunction("Add", new LuaCSFunction(Add));
		L.RegFunction("Remove", new LuaCSFunction(Remove));
		L.RegFunction("Set", new LuaCSFunction(Set));
		L.RegFunction("Clear", new LuaCSFunction(Clear));
		L.RegFunction("Call", new LuaCSFunction(Call));
		L.RegFunction("BubbleCall", new LuaCSFunction(BubbleCall));
		L.RegFunction("BroadcastCall", new LuaCSFunction(BroadcastCall));
		L.RegFunction("New", new LuaCSFunction(_CreateFairyGUI_EventListener));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("type", new LuaCSFunction(get_type), null);
		L.RegVar("isEmpty", new LuaCSFunction(get_isEmpty), null);
		L.RegVar("isDispatching", new LuaCSFunction(get_isDispatching), null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_EventListener(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.EventDispatcher arg0 = (FairyGUI.EventDispatcher)ToLua.CheckObject<FairyGUI.EventDispatcher>(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				FairyGUI.EventListener obj = new FairyGUI.EventListener(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.EventListener.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddCapture(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
			FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.CheckDelegate<FairyGUI.EventCallback1>(L, 2);
			obj.AddCapture(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveCapture(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
			FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.CheckDelegate<FairyGUI.EventCallback1>(L, 2);
			obj.RemoveCapture(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Add(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.CheckDelegate<FairyGUI.EventCallback1>(L, 2);
				obj.Add(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<LuaInterface.LuaTable>(L, 3))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
				LuaTable arg1 = ToLua.ToLuaTable(L, 3);
				obj.Add(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<FairyGUI.GComponent>(L, 3))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
				FairyGUI.GComponent arg1 = (FairyGUI.GComponent)ToLua.ToObject(L, 3);
				obj.Add(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.EventListener.Add");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Remove(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.CheckDelegate<FairyGUI.EventCallback1>(L, 2);
				obj.Remove(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<LuaInterface.LuaTable>(L, 3))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
				LuaTable arg1 = ToLua.ToLuaTable(L, 3);
				obj.Remove(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<FairyGUI.GComponent>(L, 3))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
				FairyGUI.GComponent arg1 = (FairyGUI.GComponent)ToLua.ToObject(L, 3);
				obj.Remove(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.EventListener.Remove");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Set(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.CheckDelegate<FairyGUI.EventCallback1>(L, 2);
				obj.Set(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<LuaInterface.LuaTable>(L, 3))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
				LuaTable arg1 = ToLua.ToLuaTable(L, 3);
				obj.Set(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<FairyGUI.GComponent>(L, 3))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
				FairyGUI.GComponent arg1 = (FairyGUI.GComponent)ToLua.ToObject(L, 3);
				obj.Set(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.EventListener.Set");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
			obj.Clear();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Call(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				bool o = obj.Call();
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 2)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				object arg0 = ToLua.ToVarObject(L, 2);
				bool o = obj.Call(arg0);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.EventListener.Call");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BubbleCall(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				bool o = obj.BubbleCall();
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 2)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				object arg0 = ToLua.ToVarObject(L, 2);
				bool o = obj.BubbleCall(arg0);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.EventListener.BubbleCall");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BroadcastCall(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				bool o = obj.BroadcastCall();
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else if (count == 2)
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				object arg0 = ToLua.ToVarObject(L, 2);
				bool o = obj.BroadcastCall(arg0);
				LuaDLL.lua_pushboolean(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.EventListener.BroadcastCall");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_type(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.EventListener obj = (FairyGUI.EventListener)o;
			string ret = obj.type;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index type on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isEmpty(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.EventListener obj = (FairyGUI.EventListener)o;
			bool ret = obj.isEmpty;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isEmpty on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isDispatching(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.EventListener obj = (FairyGUI.EventListener)o;
			bool ret = obj.isDispatching;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isDispatching on a nil value");
		}
	}
}


﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_EventListenerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.EventListener), typeof(System.Object));
		L.RegFunction("AddCapture", AddCapture);
		L.RegFunction("RemoveCapture", RemoveCapture);
		L.RegFunction("Add", Add);
		L.RegFunction("Remove", Remove);
		L.RegFunction("Set", Set);
		L.RegFunction("Clear", Clear);
		L.RegFunction("Call", Call);
		L.RegFunction("BubbleCall", BubbleCall);
		L.RegFunction("BroadcastCall", BroadcastCall);
		L.RegFunction("New", _CreateFairyGUI_EventListener);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("type", get_type, null);
		L.RegVar("isEmpty", get_isEmpty, null);
		L.RegVar("isDispatching", get_isDispatching, null);
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

			if (count == 2 && TypeChecker.CheckTypes<FairyGUI.EventCallback1>(L, 2))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.ToObject(L, 2);
				obj.Add(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.EventCallback0>(L, 2))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback0 arg0 = (FairyGUI.EventCallback0)ToLua.ToObject(L, 2);
				obj.Add(arg0);
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

			if (count == 2 && TypeChecker.CheckTypes<FairyGUI.EventCallback1>(L, 2))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.ToObject(L, 2);
				obj.Remove(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.EventCallback0>(L, 2))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback0 arg0 = (FairyGUI.EventCallback0)ToLua.ToObject(L, 2);
				obj.Remove(arg0);
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

			if (count == 2 && TypeChecker.CheckTypes<FairyGUI.EventCallback1>(L, 2))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback1 arg0 = (FairyGUI.EventCallback1)ToLua.ToObject(L, 2);
				obj.Set(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes<FairyGUI.EventCallback0>(L, 2))
			{
				FairyGUI.EventListener obj = (FairyGUI.EventListener)ToLua.CheckObject<FairyGUI.EventListener>(L, 1);
				FairyGUI.EventCallback0 arg0 = (FairyGUI.EventCallback0)ToLua.ToObject(L, 2);
				obj.Set(arg0);
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

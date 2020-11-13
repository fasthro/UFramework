﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GTweenerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GTweener), typeof(System.Object));
		L.RegFunction("SetDelay", new LuaCSFunction(SetDelay));
		L.RegFunction("SetDuration", new LuaCSFunction(SetDuration));
		L.RegFunction("SetBreakpoint", new LuaCSFunction(SetBreakpoint));
		L.RegFunction("SetEase", new LuaCSFunction(SetEase));
		L.RegFunction("SetEasePeriod", new LuaCSFunction(SetEasePeriod));
		L.RegFunction("SetEaseOvershootOrAmplitude", new LuaCSFunction(SetEaseOvershootOrAmplitude));
		L.RegFunction("SetRepeat", new LuaCSFunction(SetRepeat));
		L.RegFunction("SetTimeScale", new LuaCSFunction(SetTimeScale));
		L.RegFunction("SetIgnoreEngineTimeScale", new LuaCSFunction(SetIgnoreEngineTimeScale));
		L.RegFunction("SetSnapping", new LuaCSFunction(SetSnapping));
		L.RegFunction("SetPath", new LuaCSFunction(SetPath));
		L.RegFunction("SetTarget", new LuaCSFunction(SetTarget));
		L.RegFunction("SetUserData", new LuaCSFunction(SetUserData));
		L.RegFunction("OnUpdate", new LuaCSFunction(OnUpdate));
		L.RegFunction("OnStart", new LuaCSFunction(OnStart));
		L.RegFunction("OnComplete", new LuaCSFunction(OnComplete));
		L.RegFunction("SetListener", new LuaCSFunction(SetListener));
		L.RegFunction("SetPaused", new LuaCSFunction(SetPaused));
		L.RegFunction("Seek", new LuaCSFunction(Seek));
		L.RegFunction("Kill", new LuaCSFunction(Kill));
		L.RegFunction("New", new LuaCSFunction(_CreateFairyGUI_GTweener));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("delay", new LuaCSFunction(get_delay), null);
		L.RegVar("duration", new LuaCSFunction(get_duration), null);
		L.RegVar("repeat", new LuaCSFunction(get_repeat), null);
		L.RegVar("target", new LuaCSFunction(get_target), null);
		L.RegVar("userData", new LuaCSFunction(get_userData), null);
		L.RegVar("startValue", new LuaCSFunction(get_startValue), null);
		L.RegVar("endValue", new LuaCSFunction(get_endValue), null);
		L.RegVar("value", new LuaCSFunction(get_value), null);
		L.RegVar("deltaValue", new LuaCSFunction(get_deltaValue), null);
		L.RegVar("normalizedTime", new LuaCSFunction(get_normalizedTime), null);
		L.RegVar("completed", new LuaCSFunction(get_completed), null);
		L.RegVar("allCompleted", new LuaCSFunction(get_allCompleted), null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GTweener(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.GTweener obj = new FairyGUI.GTweener();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GTweener.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetDelay(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			FairyGUI.GTweener o = obj.SetDelay(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetDuration(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			FairyGUI.GTweener o = obj.SetDuration(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetBreakpoint(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			FairyGUI.GTweener o = obj.SetBreakpoint(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetEase(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				FairyGUI.EaseType arg0 = (FairyGUI.EaseType)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.EaseType>.type);
				FairyGUI.GTweener o = obj.SetEase(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				FairyGUI.EaseType arg0 = (FairyGUI.EaseType)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.EaseType>.type);
				FairyGUI.CustomEase arg1 = (FairyGUI.CustomEase)ToLua.CheckObject<FairyGUI.CustomEase>(L, 3);
				FairyGUI.GTweener o = obj.SetEase(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GTweener.SetEase");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetEasePeriod(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			FairyGUI.GTweener o = obj.SetEasePeriod(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetEaseOvershootOrAmplitude(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			FairyGUI.GTweener o = obj.SetEaseOvershootOrAmplitude(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRepeat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				FairyGUI.GTweener o = obj.SetRepeat(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				FairyGUI.GTweener o = obj.SetRepeat(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GTweener.SetRepeat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTimeScale(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			FairyGUI.GTweener o = obj.SetTimeScale(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetIgnoreEngineTimeScale(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FairyGUI.GTweener o = obj.SetIgnoreEngineTimeScale(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSnapping(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FairyGUI.GTweener o = obj.SetSnapping(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			FairyGUI.GPath arg0 = (FairyGUI.GPath)ToLua.CheckObject<FairyGUI.GPath>(L, 2);
			FairyGUI.GTweener o = obj.SetPath(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTarget(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				object arg0 = ToLua.ToVarObject(L, 2);
				FairyGUI.GTweener o = obj.SetTarget(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				object arg0 = ToLua.ToVarObject(L, 2);
				FairyGUI.TweenPropType arg1 = (FairyGUI.TweenPropType)ToLua.CheckObject(L, 3, TypeTraits<FairyGUI.TweenPropType>.type);
				FairyGUI.GTweener o = obj.SetTarget(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GTweener.SetTarget");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetUserData(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			object arg0 = ToLua.ToVarObject(L, 2);
			FairyGUI.GTweener o = obj.SetUserData(arg0);
			ToLua.PushObject(L, o);
			return 1;
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
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			FairyGUI.GTweenCallback1 arg0 = (FairyGUI.GTweenCallback1)ToLua.CheckDelegate<FairyGUI.GTweenCallback1>(L, 2);
			FairyGUI.GTweener o = obj.OnUpdate(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnStart(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			FairyGUI.GTweenCallback1 arg0 = (FairyGUI.GTweenCallback1)ToLua.CheckDelegate<FairyGUI.GTweenCallback1>(L, 2);
			FairyGUI.GTweener o = obj.OnStart(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnComplete(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			FairyGUI.GTweenCallback1 arg0 = (FairyGUI.GTweenCallback1)ToLua.CheckDelegate<FairyGUI.GTweenCallback1>(L, 2);
			FairyGUI.GTweener o = obj.OnComplete(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetListener(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			FairyGUI.ITweenListener arg0 = (FairyGUI.ITweenListener)ToLua.CheckObject<FairyGUI.ITweenListener>(L, 2);
			FairyGUI.GTweener o = obj.SetListener(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPaused(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			FairyGUI.GTweener o = obj.SetPaused(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Seek(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.Seek(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Kill(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				obj.Kill();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.GTweener obj = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				obj.Kill(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GTweener.Kill");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_delay(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			float ret = obj.delay;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index delay on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_duration(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			float ret = obj.duration;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index duration on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_repeat(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			int ret = obj.repeat;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index repeat on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_target(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			object ret = obj.target;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index target on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_userData(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			object ret = obj.userData;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index userData on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_startValue(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			FairyGUI.TweenValue ret = obj.startValue;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index startValue on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_endValue(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			FairyGUI.TweenValue ret = obj.endValue;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index endValue on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_value(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			FairyGUI.TweenValue ret = obj.value;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index value on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_deltaValue(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			FairyGUI.TweenValue ret = obj.deltaValue;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index deltaValue on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_normalizedTime(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			float ret = obj.normalizedTime;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index normalizedTime on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_completed(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			bool ret = obj.completed;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index completed on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_allCompleted(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GTweener obj = (FairyGUI.GTweener)o;
			bool ret = obj.allCompleted;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index allCompleted on a nil value");
		}
	}
}


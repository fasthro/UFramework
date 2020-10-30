﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_TransitionWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.Transition), typeof(System.Object));
		L.RegFunction("Play", new LuaCSFunction(Play));
		L.RegFunction("PlayReverse", new LuaCSFunction(PlayReverse));
		L.RegFunction("ChangePlayTimes", new LuaCSFunction(ChangePlayTimes));
		L.RegFunction("SetAutoPlay", new LuaCSFunction(SetAutoPlay));
		L.RegFunction("Stop", new LuaCSFunction(Stop));
		L.RegFunction("SetPaused", new LuaCSFunction(SetPaused));
		L.RegFunction("Dispose", new LuaCSFunction(Dispose));
		L.RegFunction("SetValue", new LuaCSFunction(SetValue));
		L.RegFunction("SetHook", new LuaCSFunction(SetHook));
		L.RegFunction("ClearHooks", new LuaCSFunction(ClearHooks));
		L.RegFunction("SetTarget", new LuaCSFunction(SetTarget));
		L.RegFunction("SetDuration", new LuaCSFunction(SetDuration));
		L.RegFunction("GetLabelTime", new LuaCSFunction(GetLabelTime));
		L.RegFunction("OnTweenStart", new LuaCSFunction(OnTweenStart));
		L.RegFunction("OnTweenUpdate", new LuaCSFunction(OnTweenUpdate));
		L.RegFunction("OnTweenComplete", new LuaCSFunction(OnTweenComplete));
		L.RegFunction("Setup", new LuaCSFunction(Setup));
		L.RegFunction("New", new LuaCSFunction(_CreateFairyGUI_Transition));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("invalidateBatchingEveryFrame", new LuaCSFunction(get_invalidateBatchingEveryFrame), new LuaCSFunction(set_invalidateBatchingEveryFrame));
		L.RegVar("name", new LuaCSFunction(get_name), null);
		L.RegVar("playing", new LuaCSFunction(get_playing), null);
		L.RegVar("timeScale", new LuaCSFunction(get_timeScale), new LuaCSFunction(set_timeScale));
		L.RegVar("ignoreEngineTimeScale", new LuaCSFunction(get_ignoreEngineTimeScale), new LuaCSFunction(set_ignoreEngineTimeScale));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_Transition(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GComponent arg0 = (FairyGUI.GComponent)ToLua.CheckObject<FairyGUI.GComponent>(L, 1);
				FairyGUI.Transition obj = new FairyGUI.Transition(arg0);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.Transition.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Play(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				obj.Play();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				FairyGUI.PlayCompleteCallback arg0 = (FairyGUI.PlayCompleteCallback)ToLua.CheckDelegate<FairyGUI.PlayCompleteCallback>(L, 2);
				obj.Play(arg0);
				return 0;
			}
			else if (count == 4)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				FairyGUI.PlayCompleteCallback arg2 = (FairyGUI.PlayCompleteCallback)ToLua.CheckDelegate<FairyGUI.PlayCompleteCallback>(L, 4);
				obj.Play(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 6)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
				float arg3 = (float)LuaDLL.luaL_checknumber(L, 5);
				FairyGUI.PlayCompleteCallback arg4 = (FairyGUI.PlayCompleteCallback)ToLua.CheckDelegate<FairyGUI.PlayCompleteCallback>(L, 6);
				obj.Play(arg0, arg1, arg2, arg3, arg4);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.Transition.Play");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayReverse(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				obj.PlayReverse();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				FairyGUI.PlayCompleteCallback arg0 = (FairyGUI.PlayCompleteCallback)ToLua.CheckDelegate<FairyGUI.PlayCompleteCallback>(L, 2);
				obj.PlayReverse(arg0);
				return 0;
			}
			else if (count == 4)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				FairyGUI.PlayCompleteCallback arg2 = (FairyGUI.PlayCompleteCallback)ToLua.CheckDelegate<FairyGUI.PlayCompleteCallback>(L, 4);
				obj.PlayReverse(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.Transition.PlayReverse");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangePlayTimes(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.ChangePlayTimes(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoPlay(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
			float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
			obj.SetAutoPlay(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Stop(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				obj.Stop();
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
				bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.Stop(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.Transition.Stop");
			}
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
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.SetPaused(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			obj.Dispose();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetValue(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			object[] arg1 = ToLua.ToParamsObject(L, 3, count - 2);
			obj.SetValue(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetHook(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.TransitionHook arg1 = (FairyGUI.TransitionHook)ToLua.CheckDelegate<FairyGUI.TransitionHook>(L, 3);
			obj.SetHook(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearHooks(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			obj.ClearHooks();
			return 0;
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
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
			obj.SetTarget(arg0, arg1);
			return 0;
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
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
			obj.SetDuration(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLabelTime(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			float o = obj.GetLabelTime(arg0);
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnTweenStart(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			FairyGUI.GTweener arg0 = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 2);
			obj.OnTweenStart(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnTweenUpdate(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			FairyGUI.GTweener arg0 = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 2);
			obj.OnTweenUpdate(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnTweenComplete(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			FairyGUI.GTweener arg0 = (FairyGUI.GTweener)ToLua.CheckObject<FairyGUI.GTweener>(L, 2);
			obj.OnTweenComplete(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Setup(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.Transition obj = (FairyGUI.Transition)ToLua.CheckObject<FairyGUI.Transition>(L, 1);
			FairyGUI.Utils.ByteBuffer arg0 = (FairyGUI.Utils.ByteBuffer)ToLua.CheckObject<FairyGUI.Utils.ByteBuffer>(L, 2);
			obj.Setup(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_invalidateBatchingEveryFrame(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			bool ret = obj.invalidateBatchingEveryFrame;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index invalidateBatchingEveryFrame on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_name(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			string ret = obj.name;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index name on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_playing(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			bool ret = obj.playing;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index playing on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_timeScale(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			float ret = obj.timeScale;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index timeScale on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ignoreEngineTimeScale(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			bool ret = obj.ignoreEngineTimeScale;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ignoreEngineTimeScale on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_invalidateBatchingEveryFrame(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.invalidateBatchingEveryFrame = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index invalidateBatchingEveryFrame on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_timeScale(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.timeScale = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index timeScale on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ignoreEngineTimeScale(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.Transition obj = (FairyGUI.Transition)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.ignoreEngineTimeScale = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index ignoreEngineTimeScale on a nil value");
		}
	}
}


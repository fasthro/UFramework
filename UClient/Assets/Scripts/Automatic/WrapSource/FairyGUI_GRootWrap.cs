﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GRootWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GRoot), typeof(FairyGUI.GComponent));
		L.RegFunction("Dispose", new LuaCSFunction(Dispose));
		L.RegFunction("SetContentScaleFactor", new LuaCSFunction(SetContentScaleFactor));
		L.RegFunction("ApplyContentScaleFactor", new LuaCSFunction(ApplyContentScaleFactor));
		L.RegFunction("ShowWindow", new LuaCSFunction(ShowWindow));
		L.RegFunction("HideWindow", new LuaCSFunction(HideWindow));
		L.RegFunction("HideWindowImmediately", new LuaCSFunction(HideWindowImmediately));
		L.RegFunction("BringToFront", new LuaCSFunction(BringToFront));
		L.RegFunction("ShowModalWait", new LuaCSFunction(ShowModalWait));
		L.RegFunction("CloseModalWait", new LuaCSFunction(CloseModalWait));
		L.RegFunction("CloseAllExceptModals", new LuaCSFunction(CloseAllExceptModals));
		L.RegFunction("CloseAllWindows", new LuaCSFunction(CloseAllWindows));
		L.RegFunction("GetTopWindow", new LuaCSFunction(GetTopWindow));
		L.RegFunction("DisplayObjectToGObject", new LuaCSFunction(DisplayObjectToGObject));
		L.RegFunction("ShowPopup", new LuaCSFunction(ShowPopup));
		L.RegFunction("GetPoupPosition", new LuaCSFunction(GetPoupPosition));
		L.RegFunction("TogglePopup", new LuaCSFunction(TogglePopup));
		L.RegFunction("HidePopup", new LuaCSFunction(HidePopup));
		L.RegFunction("ShowTooltips", new LuaCSFunction(ShowTooltips));
		L.RegFunction("ShowTooltipsWin", new LuaCSFunction(ShowTooltipsWin));
		L.RegFunction("HideTooltips", new LuaCSFunction(HideTooltips));
		L.RegFunction("EnableSound", new LuaCSFunction(EnableSound));
		L.RegFunction("DisableSound", new LuaCSFunction(DisableSound));
		L.RegFunction("PlayOneShotSound", new LuaCSFunction(PlayOneShotSound));
		L.RegFunction("New", new LuaCSFunction(_CreateFairyGUI_GRoot));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("contentScaleFactor", new LuaCSFunction(get_contentScaleFactor), null);
		L.RegVar("contentScaleLevel", new LuaCSFunction(get_contentScaleLevel), null);
		L.RegVar("inst", new LuaCSFunction(get_inst), null);
		L.RegVar("modalLayer", new LuaCSFunction(get_modalLayer), null);
		L.RegVar("hasModalWindow", new LuaCSFunction(get_hasModalWindow), null);
		L.RegVar("modalWaiting", new LuaCSFunction(get_modalWaiting), null);
		L.RegVar("touchTarget", new LuaCSFunction(get_touchTarget), null);
		L.RegVar("hasAnyPopup", new LuaCSFunction(get_hasAnyPopup), null);
		L.RegVar("focus", new LuaCSFunction(get_focus), new LuaCSFunction(set_focus));
		L.RegVar("soundVolume", new LuaCSFunction(get_soundVolume), new LuaCSFunction(set_soundVolume));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GRoot(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.GRoot obj = new FairyGUI.GRoot();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GRoot.New");
			}
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
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.Dispose();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetContentScaleFactor(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
				obj.SetContentScaleFactor(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
				obj.SetContentScaleFactor(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
				FairyGUI.UIContentScaler.ScreenMatchMode arg2 = (FairyGUI.UIContentScaler.ScreenMatchMode)ToLua.CheckObject(L, 4, TypeTraits<FairyGUI.UIContentScaler.ScreenMatchMode>.type);
				obj.SetContentScaleFactor(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.SetContentScaleFactor");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ApplyContentScaleFactor(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.ApplyContentScaleFactor();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowWindow(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			FairyGUI.Window arg0 = (FairyGUI.Window)ToLua.CheckObject<FairyGUI.Window>(L, 2);
			obj.ShowWindow(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HideWindow(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			FairyGUI.Window arg0 = (FairyGUI.Window)ToLua.CheckObject<FairyGUI.Window>(L, 2);
			obj.HideWindow(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HideWindowImmediately(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.Window arg0 = (FairyGUI.Window)ToLua.CheckObject<FairyGUI.Window>(L, 2);
				obj.HideWindowImmediately(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.Window arg0 = (FairyGUI.Window)ToLua.CheckObject<FairyGUI.Window>(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.HideWindowImmediately(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.HideWindowImmediately");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BringToFront(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			FairyGUI.Window arg0 = (FairyGUI.Window)ToLua.CheckObject<FairyGUI.Window>(L, 2);
			obj.BringToFront(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowModalWait(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.ShowModalWait();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseModalWait(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.CloseModalWait();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseAllExceptModals(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.CloseAllExceptModals();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseAllWindows(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.CloseAllWindows();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTopWindow(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			FairyGUI.Window o = obj.GetTopWindow();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DisplayObjectToGObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			FairyGUI.DisplayObject arg0 = (FairyGUI.DisplayObject)ToLua.CheckObject<FairyGUI.DisplayObject>(L, 2);
			FairyGUI.GObject o = obj.DisplayObjectToGObject(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowPopup(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				obj.ShowPopup(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
				obj.ShowPopup(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
				FairyGUI.PopupDirection arg2 = (FairyGUI.PopupDirection)ToLua.CheckObject(L, 4, TypeTraits<FairyGUI.PopupDirection>.type);
				obj.ShowPopup(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 5)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
				FairyGUI.PopupDirection arg2 = (FairyGUI.PopupDirection)ToLua.CheckObject(L, 4, TypeTraits<FairyGUI.PopupDirection>.type);
				bool arg3 = LuaDLL.luaL_checkboolean(L, 5);
				obj.ShowPopup(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.ShowPopup");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPoupPosition(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
			FairyGUI.PopupDirection arg2 = (FairyGUI.PopupDirection)ToLua.CheckObject(L, 4, TypeTraits<FairyGUI.PopupDirection>.type);
			UnityEngine.Vector2 o = obj.GetPoupPosition(arg0, arg1, arg2);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TogglePopup(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				obj.TogglePopup(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
				obj.TogglePopup(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
				FairyGUI.PopupDirection arg2 = (FairyGUI.PopupDirection)ToLua.CheckObject(L, 4, TypeTraits<FairyGUI.PopupDirection>.type);
				obj.TogglePopup(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 5)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				FairyGUI.GObject arg1 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 3);
				FairyGUI.PopupDirection arg2 = (FairyGUI.PopupDirection)ToLua.CheckObject(L, 4, TypeTraits<FairyGUI.PopupDirection>.type);
				bool arg3 = LuaDLL.luaL_checkboolean(L, 5);
				obj.TogglePopup(arg0, arg1, arg2, arg3);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.TogglePopup");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HidePopup(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				obj.HidePopup();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				obj.HidePopup(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.HidePopup");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTooltips(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.ShowTooltips(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				obj.ShowTooltips(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.ShowTooltips");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTooltipsWin(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				obj.ShowTooltipsWin(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				obj.ShowTooltipsWin(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.ShowTooltipsWin");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HideTooltips(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.HideTooltips();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableSound(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.EnableSound();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DisableSound(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
			obj.DisableSound();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayOneShotSound(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				UnityEngine.AudioClip arg0 = (UnityEngine.AudioClip)ToLua.CheckObject<UnityEngine.AudioClip>(L, 2);
				obj.PlayOneShotSound(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GRoot obj = (FairyGUI.GRoot)ToLua.CheckObject<FairyGUI.GRoot>(L, 1);
				UnityEngine.AudioClip arg0 = (UnityEngine.AudioClip)ToLua.CheckObject<UnityEngine.AudioClip>(L, 2);
				float arg1 = (float)LuaDLL.luaL_checknumber(L, 3);
				obj.PlayOneShotSound(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GRoot.PlayOneShotSound");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_contentScaleFactor(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushnumber(L, FairyGUI.GRoot.contentScaleFactor);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_contentScaleLevel(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushinteger(L, FairyGUI.GRoot.contentScaleLevel);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_inst(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, FairyGUI.GRoot.inst);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_modalLayer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			FairyGUI.GGraph ret = obj.modalLayer;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index modalLayer on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hasModalWindow(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			bool ret = obj.hasModalWindow;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index hasModalWindow on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_modalWaiting(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			bool ret = obj.modalWaiting;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index modalWaiting on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_touchTarget(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			FairyGUI.GObject ret = obj.touchTarget;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index touchTarget on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hasAnyPopup(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			bool ret = obj.hasAnyPopup;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index hasAnyPopup on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_focus(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			FairyGUI.GObject ret = obj.focus;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index focus on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_soundVolume(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			float ret = obj.soundVolume;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index soundVolume on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_focus(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			obj.focus = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index focus on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_soundVolume(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GRoot obj = (FairyGUI.GRoot)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.soundVolume = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index soundVolume on a nil value");
		}
	}
}


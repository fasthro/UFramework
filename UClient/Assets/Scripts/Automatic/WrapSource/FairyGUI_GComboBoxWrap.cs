﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GComboBoxWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GComboBox), typeof(FairyGUI.GComponent));
		L.RegFunction("ApplyListChange", new LuaCSFunction(ApplyListChange));
		L.RegFunction("GetTextField", new LuaCSFunction(GetTextField));
		L.RegFunction("HandleControllerChanged", new LuaCSFunction(HandleControllerChanged));
		L.RegFunction("Dispose", new LuaCSFunction(Dispose));
		L.RegFunction("Setup_AfterAdd", new LuaCSFunction(Setup_AfterAdd));
		L.RegFunction("UpdateDropdownList", new LuaCSFunction(UpdateDropdownList));
		L.RegFunction("New", new LuaCSFunction(_CreateFairyGUI_GComboBox));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("visibleItemCount", new LuaCSFunction(get_visibleItemCount), new LuaCSFunction(set_visibleItemCount));
		L.RegVar("dropdown", new LuaCSFunction(get_dropdown), new LuaCSFunction(set_dropdown));
		L.RegVar("sound", new LuaCSFunction(get_sound), new LuaCSFunction(set_sound));
		L.RegVar("soundVolumeScale", new LuaCSFunction(get_soundVolumeScale), new LuaCSFunction(set_soundVolumeScale));
		L.RegVar("onChanged", new LuaCSFunction(get_onChanged), null);
		L.RegVar("icon", new LuaCSFunction(get_icon), new LuaCSFunction(set_icon));
		L.RegVar("title", new LuaCSFunction(get_title), new LuaCSFunction(set_title));
		L.RegVar("text", new LuaCSFunction(get_text), new LuaCSFunction(set_text));
		L.RegVar("titleColor", new LuaCSFunction(get_titleColor), new LuaCSFunction(set_titleColor));
		L.RegVar("titleFontSize", new LuaCSFunction(get_titleFontSize), new LuaCSFunction(set_titleFontSize));
		L.RegVar("items", new LuaCSFunction(get_items), new LuaCSFunction(set_items));
		L.RegVar("icons", new LuaCSFunction(get_icons), new LuaCSFunction(set_icons));
		L.RegVar("values", new LuaCSFunction(get_values), new LuaCSFunction(set_values));
		L.RegVar("itemList", new LuaCSFunction(get_itemList), null);
		L.RegVar("valueList", new LuaCSFunction(get_valueList), null);
		L.RegVar("iconList", new LuaCSFunction(get_iconList), null);
		L.RegVar("selectedIndex", new LuaCSFunction(get_selectedIndex), new LuaCSFunction(set_selectedIndex));
		L.RegVar("selectionController", new LuaCSFunction(get_selectionController), new LuaCSFunction(set_selectionController));
		L.RegVar("value", new LuaCSFunction(get_value), new LuaCSFunction(set_value));
		L.RegVar("popupDirection", new LuaCSFunction(get_popupDirection), new LuaCSFunction(set_popupDirection));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GComboBox(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.GComboBox obj = new FairyGUI.GComboBox();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GComboBox.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ApplyListChange(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)ToLua.CheckObject<FairyGUI.GComboBox>(L, 1);
			obj.ApplyListChange();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTextField(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)ToLua.CheckObject<FairyGUI.GComboBox>(L, 1);
			FairyGUI.GTextField o = obj.GetTextField();
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandleControllerChanged(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)ToLua.CheckObject<FairyGUI.GComboBox>(L, 1);
			FairyGUI.Controller arg0 = (FairyGUI.Controller)ToLua.CheckObject<FairyGUI.Controller>(L, 2);
			obj.HandleControllerChanged(arg0);
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
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)ToLua.CheckObject<FairyGUI.GComboBox>(L, 1);
			obj.Dispose();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Setup_AfterAdd(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)ToLua.CheckObject<FairyGUI.GComboBox>(L, 1);
			FairyGUI.Utils.ByteBuffer arg0 = (FairyGUI.Utils.ByteBuffer)ToLua.CheckObject<FairyGUI.Utils.ByteBuffer>(L, 2);
			int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
			obj.Setup_AfterAdd(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateDropdownList(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)ToLua.CheckObject<FairyGUI.GComboBox>(L, 1);
			obj.UpdateDropdownList();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_visibleItemCount(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			int ret = obj.visibleItemCount;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index visibleItemCount on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dropdown(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.GComponent ret = obj.dropdown;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index dropdown on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sound(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.NAudioClip ret = obj.sound;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index sound on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_soundVolumeScale(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			float ret = obj.soundVolumeScale;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index soundVolumeScale on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onChanged(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.EventListener ret = obj.onChanged;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index onChanged on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_icon(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string ret = obj.icon;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index icon on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_title(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string ret = obj.title;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index title on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_text(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string ret = obj.text;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index text on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_titleColor(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			UnityEngine.Color ret = obj.titleColor;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index titleColor on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_titleFontSize(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			int ret = obj.titleFontSize;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index titleFontSize on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_items(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string[] ret = obj.items;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index items on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_icons(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string[] ret = obj.icons;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index icons on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_values(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string[] ret = obj.values;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index values on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			System.Collections.Generic.List<string> ret = obj.itemList;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemList on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_valueList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			System.Collections.Generic.List<string> ret = obj.valueList;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index valueList on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_iconList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			System.Collections.Generic.List<string> ret = obj.iconList;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index iconList on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectedIndex(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			int ret = obj.selectedIndex;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index selectedIndex on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectionController(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.Controller ret = obj.selectionController;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index selectionController on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_value(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string ret = obj.value;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index value on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_popupDirection(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.PopupDirection ret = obj.popupDirection;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index popupDirection on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_visibleItemCount(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.visibleItemCount = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index visibleItemCount on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_dropdown(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.GComponent arg0 = (FairyGUI.GComponent)ToLua.CheckObject<FairyGUI.GComponent>(L, 2);
			obj.dropdown = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index dropdown on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sound(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.NAudioClip arg0 = (FairyGUI.NAudioClip)ToLua.CheckObject<FairyGUI.NAudioClip>(L, 2);
			obj.sound = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index sound on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_soundVolumeScale(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.soundVolumeScale = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index soundVolumeScale on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_icon(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.icon = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index icon on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_title(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.title = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index title on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_text(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.text = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index text on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_titleColor(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			UnityEngine.Color arg0 = ToLua.ToColor(L, 2);
			obj.titleColor = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index titleColor on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_titleFontSize(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.titleFontSize = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index titleFontSize on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_items(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			obj.items = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index items on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_icons(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			obj.icons = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index icons on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_values(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			obj.values = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index values on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectedIndex(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.selectedIndex = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index selectedIndex on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectionController(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.Controller arg0 = (FairyGUI.Controller)ToLua.CheckObject<FairyGUI.Controller>(L, 2);
			obj.selectionController = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index selectionController on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_value(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.value = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index value on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_popupDirection(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GComboBox obj = (FairyGUI.GComboBox)o;
			FairyGUI.PopupDirection arg0 = (FairyGUI.PopupDirection)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.PopupDirection>.type);
			obj.popupDirection = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index popupDirection on a nil value");
		}
	}
}


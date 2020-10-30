﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FairyGUI_GListWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(FairyGUI.GList), typeof(FairyGUI.GComponent));
		L.RegFunction("Dispose", new LuaCSFunction(Dispose));
		L.RegFunction("GetFromPool", new LuaCSFunction(GetFromPool));
		L.RegFunction("AddItemFromPool", new LuaCSFunction(AddItemFromPool));
		L.RegFunction("AddChildAt", new LuaCSFunction(AddChildAt));
		L.RegFunction("RemoveChildAt", new LuaCSFunction(RemoveChildAt));
		L.RegFunction("RemoveChildToPoolAt", new LuaCSFunction(RemoveChildToPoolAt));
		L.RegFunction("RemoveChildToPool", new LuaCSFunction(RemoveChildToPool));
		L.RegFunction("RemoveChildrenToPool", new LuaCSFunction(RemoveChildrenToPool));
		L.RegFunction("GetSelection", new LuaCSFunction(GetSelection));
		L.RegFunction("AddSelection", new LuaCSFunction(AddSelection));
		L.RegFunction("RemoveSelection", new LuaCSFunction(RemoveSelection));
		L.RegFunction("ClearSelection", new LuaCSFunction(ClearSelection));
		L.RegFunction("SelectAll", new LuaCSFunction(SelectAll));
		L.RegFunction("SelectNone", new LuaCSFunction(SelectNone));
		L.RegFunction("SelectReverse", new LuaCSFunction(SelectReverse));
		L.RegFunction("EnableSelectionFocusEvents", new LuaCSFunction(EnableSelectionFocusEvents));
		L.RegFunction("EnableArrowKeyNavigation", new LuaCSFunction(EnableArrowKeyNavigation));
		L.RegFunction("HandleArrowKey", new LuaCSFunction(HandleArrowKey));
		L.RegFunction("ResizeToFit", new LuaCSFunction(ResizeToFit));
		L.RegFunction("HandleControllerChanged", new LuaCSFunction(HandleControllerChanged));
		L.RegFunction("ScrollToView", new LuaCSFunction(ScrollToView));
		L.RegFunction("GetFirstChildInView", new LuaCSFunction(GetFirstChildInView));
		L.RegFunction("ChildIndexToItemIndex", new LuaCSFunction(ChildIndexToItemIndex));
		L.RegFunction("ItemIndexToChildIndex", new LuaCSFunction(ItemIndexToChildIndex));
		L.RegFunction("SetVirtual", new LuaCSFunction(SetVirtual));
		L.RegFunction("SetVirtualAndLoop", new LuaCSFunction(SetVirtualAndLoop));
		L.RegFunction("RefreshVirtualList", new LuaCSFunction(RefreshVirtualList));
		L.RegFunction("Setup_BeforeAdd", new LuaCSFunction(Setup_BeforeAdd));
		L.RegFunction("Setup_AfterAdd", new LuaCSFunction(Setup_AfterAdd));
		L.RegFunction("New", new LuaCSFunction(_CreateFairyGUI_GList));
		L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
		L.RegVar("defaultItem", new LuaCSFunction(get_defaultItem), new LuaCSFunction(set_defaultItem));
		L.RegVar("foldInvisibleItems", new LuaCSFunction(get_foldInvisibleItems), new LuaCSFunction(set_foldInvisibleItems));
		L.RegVar("selectionMode", new LuaCSFunction(get_selectionMode), new LuaCSFunction(set_selectionMode));
		L.RegVar("itemRenderer", new LuaCSFunction(get_itemRenderer), new LuaCSFunction(set_itemRenderer));
		L.RegVar("itemProvider", new LuaCSFunction(get_itemProvider), new LuaCSFunction(set_itemProvider));
		L.RegVar("scrollItemToViewOnClick", new LuaCSFunction(get_scrollItemToViewOnClick), new LuaCSFunction(set_scrollItemToViewOnClick));
		L.RegVar("onClickItem", new LuaCSFunction(get_onClickItem), null);
		L.RegVar("onRightClickItem", new LuaCSFunction(get_onRightClickItem), null);
		L.RegVar("layout", new LuaCSFunction(get_layout), new LuaCSFunction(set_layout));
		L.RegVar("lineCount", new LuaCSFunction(get_lineCount), new LuaCSFunction(set_lineCount));
		L.RegVar("columnCount", new LuaCSFunction(get_columnCount), new LuaCSFunction(set_columnCount));
		L.RegVar("lineGap", new LuaCSFunction(get_lineGap), new LuaCSFunction(set_lineGap));
		L.RegVar("columnGap", new LuaCSFunction(get_columnGap), new LuaCSFunction(set_columnGap));
		L.RegVar("align", new LuaCSFunction(get_align), new LuaCSFunction(set_align));
		L.RegVar("verticalAlign", new LuaCSFunction(get_verticalAlign), new LuaCSFunction(set_verticalAlign));
		L.RegVar("autoResizeItem", new LuaCSFunction(get_autoResizeItem), new LuaCSFunction(set_autoResizeItem));
		L.RegVar("defaultItemSize", new LuaCSFunction(get_defaultItemSize), new LuaCSFunction(set_defaultItemSize));
		L.RegVar("itemPool", new LuaCSFunction(get_itemPool), null);
		L.RegVar("selectedIndex", new LuaCSFunction(get_selectedIndex), new LuaCSFunction(set_selectedIndex));
		L.RegVar("selectionController", new LuaCSFunction(get_selectionController), new LuaCSFunction(set_selectionController));
		L.RegVar("touchItem", new LuaCSFunction(get_touchItem), null);
		L.RegVar("isVirtual", new LuaCSFunction(get_isVirtual), null);
		L.RegVar("numItems", new LuaCSFunction(get_numItems), new LuaCSFunction(set_numItems));
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateFairyGUI_GList(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				FairyGUI.GList obj = new FairyGUI.GList();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: FairyGUI.GList.New");
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
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.Dispose();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFromPool(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			FairyGUI.GObject o = obj.GetFromPool(arg0);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddItemFromPool(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				FairyGUI.GObject o = obj.AddItemFromPool();
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 2)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				FairyGUI.GObject o = obj.AddItemFromPool(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GList.AddItemFromPool");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddChildAt(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
			FairyGUI.GObject o = obj.AddChildAt(arg0, arg1);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveChildAt(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				FairyGUI.GObject o = obj.RemoveChildAt(arg0);
				ToLua.PushObject(L, o);
				return 1;
			}
			else if (count == 3)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				FairyGUI.GObject o = obj.RemoveChildAt(arg0, arg1);
				ToLua.PushObject(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GList.RemoveChildAt");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveChildToPoolAt(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.RemoveChildToPoolAt(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveChildToPool(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			FairyGUI.GObject arg0 = (FairyGUI.GObject)ToLua.CheckObject<FairyGUI.GObject>(L, 2);
			obj.RemoveChildToPool(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveChildrenToPool(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				obj.RemoveChildrenToPool();
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
				obj.RemoveChildrenToPool(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GList.RemoveChildrenToPool");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSelection(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				System.Collections.Generic.List<int> o = obj.GetSelection();
				ToLua.PushSealed(L, o);
				return 1;
			}
			else if (count == 2)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				System.Collections.Generic.List<int> arg0 = (System.Collections.Generic.List<int>)ToLua.CheckObject(L, 2, TypeTraits<System.Collections.Generic.List<int>>.type);
				System.Collections.Generic.List<int> o = obj.GetSelection(arg0);
				ToLua.PushSealed(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GList.GetSelection");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddSelection(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
			obj.AddSelection(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveSelection(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.RemoveSelection(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearSelection(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.ClearSelection();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SelectAll(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.SelectAll();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SelectNone(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.SelectNone();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SelectReverse(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.SelectReverse();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableSelectionFocusEvents(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.EnableSelectionFocusEvents(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnableArrowKeyNavigation(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.EnableArrowKeyNavigation(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandleArrowKey(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			int o = obj.HandleArrowKey(arg0);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResizeToFit(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				obj.ResizeToFit();
				return 0;
			}
			else if (count == 2)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				obj.ResizeToFit(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
				obj.ResizeToFit(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GList.ResizeToFit");
			}
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
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
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
	static int ScrollToView(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				obj.ScrollToView(arg0);
				return 0;
			}
			else if (count == 3)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				obj.ScrollToView(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
				int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
				bool arg1 = LuaDLL.luaL_checkboolean(L, 3);
				bool arg2 = LuaDLL.luaL_checkboolean(L, 4);
				obj.ScrollToView(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FairyGUI.GList.ScrollToView");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFirstChildInView(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int o = obj.GetFirstChildInView();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChildIndexToItemIndex(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			int o = obj.ChildIndexToItemIndex(arg0);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ItemIndexToChildIndex(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			int o = obj.ItemIndexToChildIndex(arg0);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetVirtual(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.SetVirtual();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetVirtualAndLoop(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.SetVirtualAndLoop();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshVirtualList(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			obj.RefreshVirtualList();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Setup_BeforeAdd(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
			FairyGUI.Utils.ByteBuffer arg0 = (FairyGUI.Utils.ByteBuffer)ToLua.CheckObject<FairyGUI.Utils.ByteBuffer>(L, 2);
			int arg1 = (int)LuaDLL.luaL_checkinteger(L, 3);
			obj.Setup_BeforeAdd(arg0, arg1);
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
			FairyGUI.GList obj = (FairyGUI.GList)ToLua.CheckObject<FairyGUI.GList>(L, 1);
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
	static int get_defaultItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			string ret = obj.defaultItem;
			LuaDLL.lua_pushstring(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index defaultItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_foldInvisibleItems(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool ret = obj.foldInvisibleItems;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index foldInvisibleItems on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectionMode(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListSelectionMode ret = obj.selectionMode;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index selectionMode on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemRenderer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListItemRenderer ret = obj.itemRenderer;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemRenderer on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemProvider(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListItemProvider ret = obj.itemProvider;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemProvider on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_scrollItemToViewOnClick(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool ret = obj.scrollItemToViewOnClick;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index scrollItemToViewOnClick on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onClickItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.EventListener ret = obj.onClickItem;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index onClickItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onRightClickItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.EventListener ret = obj.onRightClickItem;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index onRightClickItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_layout(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListLayoutType ret = obj.layout;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index layout on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lineCount(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int ret = obj.lineCount;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index lineCount on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_columnCount(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int ret = obj.columnCount;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index columnCount on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lineGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int ret = obj.lineGap;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index lineGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_columnGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int ret = obj.columnGap;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index columnGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_align(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.AlignType ret = obj.align;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index align on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_verticalAlign(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.VertAlignType ret = obj.verticalAlign;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index verticalAlign on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_autoResizeItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool ret = obj.autoResizeItem;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index autoResizeItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_defaultItemSize(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			UnityEngine.Vector2 ret = obj.defaultItemSize;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index defaultItemSize on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_itemPool(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.GObjectPool ret = obj.itemPool;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemPool on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_selectedIndex(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
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
			FairyGUI.GList obj = (FairyGUI.GList)o;
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
	static int get_touchItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.GObject ret = obj.touchItem;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index touchItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isVirtual(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool ret = obj.isVirtual;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index isVirtual on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_numItems(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int ret = obj.numItems;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index numItems on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_defaultItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			string arg0 = ToLua.CheckString(L, 2);
			obj.defaultItem = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index defaultItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_foldInvisibleItems(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.foldInvisibleItems = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index foldInvisibleItems on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectionMode(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListSelectionMode arg0 = (FairyGUI.ListSelectionMode)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.ListSelectionMode>.type);
			obj.selectionMode = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index selectionMode on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_itemRenderer(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListItemRenderer arg0 = (FairyGUI.ListItemRenderer)ToLua.CheckDelegate<FairyGUI.ListItemRenderer>(L, 2);

			if (!object.ReferenceEquals(obj.itemRenderer, arg0))
			{
				if (obj.itemRenderer != null) obj.itemRenderer.SubRef();
				obj.itemRenderer = arg0;
			}

			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemRenderer on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_itemProvider(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListItemProvider arg0 = (FairyGUI.ListItemProvider)ToLua.CheckDelegate<FairyGUI.ListItemProvider>(L, 2);

			if (!object.ReferenceEquals(obj.itemProvider, arg0))
			{
				if (obj.itemProvider != null) obj.itemProvider.SubRef();
				obj.itemProvider = arg0;
			}

			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index itemProvider on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_scrollItemToViewOnClick(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.scrollItemToViewOnClick = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index scrollItemToViewOnClick on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_layout(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.ListLayoutType arg0 = (FairyGUI.ListLayoutType)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.ListLayoutType>.type);
			obj.layout = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index layout on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lineCount(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.lineCount = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index lineCount on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_columnCount(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.columnCount = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index columnCount on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lineGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.lineGap = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index lineGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_columnGap(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.columnGap = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index columnGap on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_align(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.AlignType arg0 = (FairyGUI.AlignType)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.AlignType>.type);
			obj.align = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index align on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_verticalAlign(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			FairyGUI.VertAlignType arg0 = (FairyGUI.VertAlignType)ToLua.CheckObject(L, 2, TypeTraits<FairyGUI.VertAlignType>.type);
			obj.verticalAlign = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index verticalAlign on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_autoResizeItem(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.autoResizeItem = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index autoResizeItem on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_defaultItemSize(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			UnityEngine.Vector2 arg0 = ToLua.ToVector2(L, 2);
			obj.defaultItemSize = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index defaultItemSize on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_selectedIndex(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
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
			FairyGUI.GList obj = (FairyGUI.GList)o;
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
	static int set_numItems(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			FairyGUI.GList obj = (FairyGUI.GList)o;
			int arg0 = (int)LuaDLL.luaL_checkinteger(L, 2);
			obj.numItems = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index numItems on a nil value");
		}
	}
}


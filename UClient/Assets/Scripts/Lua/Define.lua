--[[
Author: fasthro
Date: 2020-10-07 17:21:14
Description: define
--]]
require("DefineEvent")

------------------------------------------- uframework csharp -------------------------------------------
_G.App = UFramework.App

-- panel layer
_G.PANEL_LAYER = {
    SCNEN = UFramework.UI.Layer.SCNEN,
    PANEL = UFramework.UI.Layer.PANEL,
    MESSAGE_BOX = UFramework.UI.Layer.MESSAGE_BOX,
    GUIDE = UFramework.UI.Layer.GUIDE,
    NOTIFICATION = UFramework.UI.Layer.NOTIFICATION,
    NETWORK = UFramework.UI.Layer.NETWORK,
    LOADER = UFramework.UI.Layer.LOADER,
    TOP = UFramework.UI.Layer.TOP
}

------------------------------------------- global -------------------------------------------
_G.LuaEngine = nil

-- manager
_G.CtrlManager = nil
_G.EventManager = nil
_G.NetManager = nil
_G.PanelManager = nil
_G.ResManager = nil

-- ctrl
_G.LoginCtrl = nil

--[[
Author: fasthro
Date: 2020-10-07 17:21:14
Description: define
--]]
require("Define.DefineManager")
require("Define.DefineCtrl")
require("Define.DefinePanel")
------------------------------------------- uframework csharp -------------------------------------------
_G._app = UFramework.App

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
_G._engine = nil
_G._managerCenter = nil

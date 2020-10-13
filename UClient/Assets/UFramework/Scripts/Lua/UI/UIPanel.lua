--[[
Author: fasthro
Date: 2020-10-07 11:38:51
Description: UIPanel
--]]
------------------------------------------------------
-- layer
local layer = {
    SCNEN = UFramework.UI.Layer.SCNEN,
    PANEL = UFramework.UI.Layer.PANEL,
    MESSAGE_BOX = UFramework.UI.Layer.MESSAGE_BOX,
    GUIDE = UFramework.UI.Layer.GUIDE,
    NOTIFICATION = UFramework.UI.Layer.NOTIFICATION,
    NETWORK = UFramework.UI.Layer.NETWORK,
    LOADER = UFramework.UI.Layer.LOADER,
    TOP = UFramework.UI.Layer.TOP
}

------------------------------------------------------
local panel =
    typesys.def.UIPanel {
    _cpanel = typesys.__unmanaged,
    _name = "",
    _layer = typesys.__unmanaged,
    _package = "",
    _dependences = typesys.__unmanaged,
    _fullScreen = false
}

function panel:__ctor()
    self._cpanel = UFramework.UI.FiaryPanel.New(self._name, self._package, self._layer)

    self._cpanel:AddPackage(self._package)
    if self._dependences ~= nil then
        for k = 1, #self._dependences do
            self._cpanel:AddPackage(self._dependences[k])
        end
    end
end

function panel:__dtor()
end

function panel:show()
    self._cpanel:Show()
end

function panel:_onShow()
end

function panel:hide()
end

function panel:_onHide()
end

function panel:sendNotification(id)
end

function panel:_onNotification(id)
end

function panel:_onNetMessage()
end
------------------------------------------------------
UI_LAYER = layer
UIPanel = panel

--[[
Author: fasthro
Date: 2020-10-07 11:38:51
Description: UIPanel
--]]
------------------------------------------------------
local panel =
    typesys.def.BasePanel {
    _cpanel = typesys.__unmanaged,
    _name = "",
    _layer = typesys.__unmanaged,
    _package = "",
    _dependences = typesys.__unmanaged,
    _fullScreen = false,
    view = typesys.__unmanaged
}

function panel:__ctor()
    self._cpanel = UFramework.UI.FiaryPanel.New(self._name, self._package, self._layer)
    self._cpanel:LuaBind(self)

    self._cpanel:AddPackage(self._package)
    if self._dependences ~= nil then
        for k = 1, #self._dependences do
            self._cpanel:AddPackage(self._dependences[k])
        end
    end

    self:_onAwake()
end

function panel:__dtor()
    self:_onDispose()
end

function panel:show()
    self._cpanel:Show()
end

function panel:hide()
end

function panel:broadcastEvent(id)
end

function panel:_onAwake()
end

function panel:_onDispose()
end

function panel:onShow()
    self.view = self._cpanel.view
end

function panel:onHide()
end

function panel:onReceiveEvent(id)
end

function panel:onReceivePack()
end

------------------------------------

function panel:_bindClick(obj, func)
    obj.onClick:Set(func, self)
end

------------------------------------
function panel:getManager(name)
    return _managerCenter:getManager(name)
end

function panel:getCtrl(name)
    return self:getManager(typesys.CtrlManager):getCtrl(name)
end

return panel

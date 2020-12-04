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
    _fullScreen = false, -- 是否全屏（如果全屏就会关闭世界相机，用于性能优化）
    _immortal = false, -- 永生不会被销毁
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

    self:onAwake()
end

function panel:__dtor()
    self:onDispose()
end

function panel:show()
    EventManager:add(EVENT_NAMES.NET_RECEIVED, self.onNetReceived, self)
    self._cpanel:Show()
end

function panel:hide()
    EventManager:remove(EVENT_NAMES.NET_RECEIVED, self.onNetReceived)
    self._cpanel:Hide()
end

function panel:onAwake()
end

function panel:onDispose()
end

function panel:onShow()
    self.view = self._cpanel.view
end

function panel:onHide()
end

function panel:onNetReceived(pack)
end

------------------------------------
function panel:_createObject(resName, packageName)
    if packageName == nil then
        packageName = self._package
    end
    return UIPackage.CreateObject(packageName, resName)
end

function panel:_bindClick(obj, func)
    obj.onClick:Set(func, self)
end

return panel

--[[
Author: fasthro
Date: 2020-11-11 15:32:31
Description: 面板管理
--]]
local PanelManager =
    typesys.def.PanelManager {
    __super = typesys.BaseManager,
    _panels = typesys.map
}

function PanelManager:initialize()
    self._panels = typesys.new(typesys.map, type(""), typesys.BasePanel)
end

function PanelManager:getPanel(name)
    local _name, _dt = __parsedtn(name)
    return self._panels:get(_name)
end

function PanelManager:showPanel(name)
    local _name, _dt = __parsedtn(name)
    local panel = self:getPanel(_name)
    if panel == nil then
        panel = __newdtn(name)
        self._panels:set(_name, panel)
    end
    panel:show()
    return panel
end

function PanelManager:hidePanel(name)
    local _name, _dt = __parsedtn(name)
    local panel = self:getPanel(_name)
    if panel ~= nil then
        panel:hide()
    end
    self._panels:set(_name, nil)
end

return PanelManager

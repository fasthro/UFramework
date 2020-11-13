--[[
Author: fasthro
Date: 2020-11-11 16:04:27
Description: 
--]]
local ManagerCenter =
    typesys.def.ManagerCenter {
    _managers = typesys.map
}

function ManagerCenter:__ctor()
    _G._managerCenter = self

    self._managers = typesys.new(typesys.map, type(""), typesys.BaseManager)

    self:_addManager(typesys.PanelManager)
    self:_addManager(typesys.CtrlManager)
end

function ManagerCenter:__dtor()
end

function ManagerCenter:_addManager(name, csharp)
    local manager, _name = __newdtn(name)
    manager:initialize()
    self._managers:set(_name, manager)
end

function ManagerCenter:getManager(name)
    local _name, _dt = __parsedtn(name)
    return self._managers:get(_name)
end

function ManagerCenter:removeManager(name)
    local _name, _dt = __parsedtn(name)
    self._managers:set(_name, nil)
end

return ManagerCenter

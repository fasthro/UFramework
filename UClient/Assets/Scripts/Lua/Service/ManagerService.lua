--[[
Author: fasthro
Date: 2020-11-11 16:04:27
Description: manager service
--]]
require("Manager.BaseManager")
require("Manager.PanelManager")
require("Manager.CtrlManager")
require("Manager.NetManager")
require("Manager.ResManager")
require("Manager.EventManager")

local ManagerService =
    typesys.def.ManagerService {
    __super = typesys.BaseService,
    _managers = typesys.map
}

function ManagerService:initialize()
    self._managers = typesys.new(typesys.map, type(""), typesys.BaseManager)

    _G.PanelManager = self:_addManager(typesys.PanelManager)
    _G.CtrlManager = self:_addManager(typesys.CtrlManager)
    _G.EventManager = self:_addManager(typesys.EventManager)
    _G.NetManager = self:_addManager(typesys.NetManager, self._ext:GetManager("NetManager"))
    _G.ResManager = self:_addManager(typesys.ResManager, self._ext:GetManager("ResManager"))
end

function ManagerService:_addManager(name, ext)
    local manager, _name = __newdtn(name, ext)
    manager:initialize()
    self._managers:set(_name, manager)
    return manager
end

function ManagerService:getManager(name)
    local _name, _dt = __parsedtn(name)
    return self._managers:get(_name)
end

function ManagerService:removeManager(name)
    local _name, _dt = __parsedtn(name)
    self._managers:set(_name, nil)
end

return ManagerService

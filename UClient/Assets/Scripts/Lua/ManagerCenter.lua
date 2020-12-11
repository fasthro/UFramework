--[[
Author: fasthro
Date: 2020-11-11 16:04:27
Description: manager center
--]]
require("Manager.BaseManager")
require("Manager.PanelManager")
require("Manager.CtrlManager")
require("Manager.NetManager")
require("Manager.ResManager")
require("Manager.EventManager")

local ManagerCenter =
    typesys.def.ManagerCenter {
    _managers = typesys.map
}

function ManagerCenter:__ctor()
    self._managers = typesys.new(typesys.map, type(""), typesys.BaseManager)

    local container = Service.container:GetService("ManagerService").container

    _G.PanelManager = self:_addManager(typesys.PanelManager)
    _G.CtrlManager = self:_addManager(typesys.CtrlManager)
    _G.EventManager = self:_addManager(typesys.EventManager)
    _G.NetManager = self:_addManager(typesys.NetManager, container:GetManager("NetManager"))
    _G.ResManager = self:_addManager(typesys.ResManager, container:GetManager("ResManager"))
end

function ManagerCenter:__dtor()
end

function ManagerCenter:_addManager(name, ext)
    local manager, _name = __newdtn(name, ext)
    manager:initialize()
    self._managers:set(_name, manager)
    return manager
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

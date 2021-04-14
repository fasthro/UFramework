--[[
Author: fasthro
Date: 2021-01-04 12:48:42
Description: 
--]]

require("Manager.BaseManager")
require("Manager.PanelManager")
require("Manager.CtrlManager")
require("Manager.NetManager")
require("Manager.ResManager")
require("Manager.EventManager")
require("Manager.ServerManager")
require("Manager.TableManager")

local Launcher =
    typesys.def.Launcher {
    _managers = typesys.map
}

function Launcher:__ctor(managerContainer)
    self._managers = typesys.new(typesys.map, type(""), typesys.BaseManager)

    _G.PanelManager = self:_addManager(typesys.PanelManager)
    _G.CtrlManager = self:_addManager(typesys.CtrlManager)
    _G.EventManager = self:_addManager(typesys.EventManager)
    _G.ServerManager = self:_addManager(typesys.ServerManager)
    _G.TableManager = self:_addManager(typesys.TableManager)
    _G.NetManager = self:_addManager(typesys.NetManager, managerContainer:GetManager("NetworkManager"))
    _G.ResManager = self:_addManager(typesys.ResManager, managerContainer:GetManager("ResManager"))
end

function Launcher:initialize()
    AlertCtrl:initialize()
    LoginCtrl:initialize()
end

function Launcher:_addManager(name, ext)
    local manager, _name = __newdtn(name, ext)
    manager:initialize()
    self._managers:set(_name, manager)
    return manager
end

function Launcher:getManager(name)
    local _name, _dt = __parsedtn(name)
    return self._managers:get(_name)
end

function Launcher:removeManager(name)
    local _name, _dt = __parsedtn(name)
    self._managers:set(_name, nil)
end

return Launcher
--[[
Author: fasthro
Date: 2020-12-28 12:15:11
Description: service
--]]
require "Service.BaseService"
require "Service.ManagerService"


local Service =
    typesys.def.Service {
    _ext = typesys.__unmanaged,
    _services = typesys.map
}

function Service:__ctor()
    self._ext = UFramework.Service.Instance
    self._services = typesys.new(typesys.map, type(""), typesys.BaseService)

    _G.ManagerService = self:_addService(typesys.ManagerService, self._ext:GetService("ManagerService"))
end

function Service:__dtor()
end

function Service:initialize()
    AlertCtrl:initialize()
    LoginCtrl:initialize()
end

function Service:_addService(name, ext)
    local service, _name = __newdtn(name, ext)
    service:initialize()
    self._services:set(_name, service)
    return service
end

function Service:getService(name)
    local _name, _dt = __parsedtn(name)
    return self._services:get(_name)
end

function Service:removeService(name)
    local _name, _dt = __parsedtn(name)
    self._services:set(_name, nil)
end

return Service

--[[
Author: fasthro
Date: 2020-11-13 14:25:40
Description: controller manager
--]]

require("Controller.BaseCtrl")
require("Controller.LoginCtrl")

local CtrlManager =
    typesys.def.CtrlManager {
    __super = typesys.BaseManager,
    _ctrls = typesys.map
}

function CtrlManager:initialize()
    self._ctrls = typesys.new(typesys.map, type(""), typesys.BaseCtrl)

    _G.LoginCtrl = self:_addCtrl(typesys.LoginCtrl)
end

function CtrlManager:_addCtrl(name)
    local _ctrl, _name = __newdtn(name)
    self._ctrls:set(_name, _ctrl)
    return _ctrl
end

function CtrlManager:getCtrl(name)
    local _name, _dt = __parsedtn(name)
    return self._ctrls:get(_name)
end

function CtrlManager:removeCtrl(name)
    local _name, _dt = __parsedtn(name)
    self._ctrls:set(_name, nil)
end

return CtrlManager

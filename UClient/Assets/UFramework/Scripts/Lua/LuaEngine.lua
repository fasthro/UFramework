--[[
Author: fasthro
Date: 2020-08-28 12:15:46
Description: main
--]]
require("Extension.TableExtension")
require("UI.FairyGUI")
require("UI.BasePanel")
require("Define")
require("Common.Function")
require("ManagerCenter")

local _new = typesys.new
local _setRootObject = typesys.setRootObject
local _typesys_gc = typesys.gc

_engine =
    typesys.def._engine {
    managerCenter = typesys.ManagerCenter
}

function _engine:__ctor()
    _engine.instance = self
end

function _engine:__dtor()
    _engine.instance = nil
end

-- static
function _engine.launch()
    _setRootObject(_new(_engine))
end

function _engine.runner()
    _engine.onRunner(_engine.instance)
end

function _engine.update()
    _engine.onUpdate(_engine.instance)
end

function _engine.lateUpdate()
    _engine.onLateUpdate(_engine.instance)
end

function _engine.fixedUpdate()
    _engine.onFixedUpdate(_engine.instance)
end

function _engine.shutdown()
    _setRootObject(nil)
end

-- members
function _engine:onRunner()
    self.managerCenter = _new(typesys.ManagerCenter)
    self.managerCenter:getManager(typesys.CtrlManager):getCtrl(typesys.LoginCtrl):initialize()
end

function _engine:onUpdate()
end

function _engine:onLateUpdate()
    _typesys_gc()
end

function _engine:onFixedUpdate()
end

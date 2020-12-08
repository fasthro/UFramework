--[[
Author: fasthro
Date: 2020-08-28 12:15:46
Description: main
--]]
require("Extension.TableExtension")
require("UI.FairyGUI")
require("UI.BasePanel")
require("Common.Const")
require("Common.EventName")
require("Common.Function")
require("Logger.Logger")
require("Define")
require("DefinePanel")
require("ManagerCenter")

local _new = typesys.new
local _setRootObject = typesys.setRootObject
local _typesys_gc = typesys.gc

LuaEngine =
    typesys.def.LuaEngine {
    managerCenter = typesys.ManagerCenter
}

function LuaEngine:__ctor()
    LuaEngine.instance = self
end

function LuaEngine:__dtor()
    LuaEngine.instance = nil
end

-- static
function LuaEngine.launch()
    _setRootObject(_new(LuaEngine))
end

function LuaEngine.runner()
    LuaEngine.onRunner(LuaEngine.instance)
end

function LuaEngine.update()
    LuaEngine.onUpdate(LuaEngine.instance)
end

function LuaEngine.lateUpdate()
    LuaEngine.onLateUpdate(LuaEngine.instance)
end

function LuaEngine.fixedUpdate()
    LuaEngine.onFixedUpdate(LuaEngine.instance)
end

function LuaEngine.shutdown()
    _setRootObject(nil)
end

-- members
function LuaEngine:onRunner()
    self.managerCenter = _new(typesys.ManagerCenter)
    AlertCtrl:initialize()
    LoginCtrl:initialize()
end

function LuaEngine:onUpdate()
end

function LuaEngine:onLateUpdate()
    _typesys_gc()
end

function LuaEngine:onFixedUpdate()
end

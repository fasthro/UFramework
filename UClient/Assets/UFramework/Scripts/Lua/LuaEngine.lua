--[[
Author: fasthro
Date: 2020-08-28 12:15:46
Description: main
--]]
_G.UApplication = UFramework.UApplication
_G.IOPath = UFramework.IOPath
_G.AppConfig = nil

require("Extension.TableExtension")
require("UI.FairyGUI")
require("UI.BasePanel")
require("Common.Const")
require("Common.EventName")
require("Common.Function")
require("Logger.Logger")
require("DefinePanel")
require("Launcher")

local _new = typesys.new
local _setRootObject = typesys.setRootObject
local _typesys_gc = typesys.gc

local _managerContainer = nil

LuaEngine =
    typesys.def.LuaEngine {
    launcher = typesys.Launcher
}

function LuaEngine:__ctor()
    LuaEngine.instance = self
end

function LuaEngine:__dtor()
    LuaEngine.instance = nil
end

-- 初始化 luaEngine
function LuaEngine.initialize(appConfig, managerContainer)
    _G.AppConfig = appConfig
    _managerContainer = managerContainer

    logger.setlevel(0)

    -- 启动LuaEngine
    _setRootObject(_new(LuaEngine))
end

function LuaEngine.launch()
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
    self.launcher = _new(typesys.Launcher, _managerContainer)
    self.launcher:initialize()
end

function LuaEngine:onUpdate()
end

function LuaEngine:onLateUpdate()
    _typesys_gc()
end

function LuaEngine:onFixedUpdate()
end

--------------------- 扩展方法 --------------------
-- Fairy 查询面板组件
-- @param panelname 面板名称
-- @param path 组件路径
function LuaEngine.fairyQueryObject(panelname, path)
    return PanelManager:fairyQueryObject(panelname, path)
end

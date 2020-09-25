--[[
Author: fasthro
Date: 2020-08-28 12:15:46
Description: Lua App
--]]

require("Define")

local _new = typesys.new
local _setRootObject = typesys.setRootObject
local _typesys_gc = typesys.gc

luaApp = typesys.def.LuaApp{
    date = "",
}

-- 构造函数
function luaApp:__ctor()
    
end

-- 析构函数
function luaApp:__dtor()

end

-- unity -> Update
function luaApp:onUpdate()
end

-- unity -> LateUpdate
function luaApp:onLateUpdate()
    _typesys_gc()
end

-- unity -> FixedUpdate
function luaApp:onFixedUpdate()
end

local onUpdate = luaApp.onUpdate
local onLateUpdate = luaApp.onLateUpdate
local onFixedUpdate = luaApp.onFixedUpdate

function luaApp.start()
    print("UFramework LuaApp Start.")
    _setRootObject(_new(luaApp))
end

function luaApp.update()
    onUpdate(luaApp.instance)
end

function luaApp.lateUpdate()
    onLateUpdate(luaApp.instance)
end

function luaApp.fixedUpdate()
    onFixedUpdate(luaApp.instance)
end

function luaApp.destory()
    print("UFramework LuaApp Destory.")
    _setRootObject(nil)
end
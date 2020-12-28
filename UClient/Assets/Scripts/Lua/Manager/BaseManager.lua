--[[
Author: fasthro
Date: 2020-11-11 16:18:40
Description: manager base class
--]]
local BaseManager =
    typesys.def.BaseManager {
    _ext = typesys.__unmanaged
}

function BaseManager:__ctor(ext)
    self._ext = ext

    if ext ~= nil then
        ext.luaTable = self
    end
end

function BaseManager:__dtor()
end

function BaseManager:initialize()
end

return BaseManager

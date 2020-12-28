--[[
Author: fasthro
Date: 2020-12-28 12:09:16
Description: 
--]]
local BaseService =
    typesys.def.BaseService {
    _ext = typesys.__unmanaged
}

function BaseService:__ctor(ext)
    self._ext = ext

    if ext ~= nil then
        ext.luaTable = self
    end
end

function BaseService:__dtor()
end

function BaseService:initialize()
end

return BaseService
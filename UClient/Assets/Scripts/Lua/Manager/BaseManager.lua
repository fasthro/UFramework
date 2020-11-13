--[[
Author: fasthro
Date: 2020-11-11 16:18:40
Description: manager base class
--]]
local BaseManager = typesys.def.BaseManager {}

function BaseManager:__ctor()
end

function BaseManager:__dtor()
    print("-----------------------------mmmmmmmmmmmmmmmmm")
end

function BaseManager:initialize()
end

return BaseManager

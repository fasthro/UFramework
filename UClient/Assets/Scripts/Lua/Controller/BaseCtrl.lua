--[[
Author: fasthro
Date: 2020-11-11 16:18:40
Description: ctrl base class
--]]
local BaseCtrl = typesys.def.BaseCtrl {}

function BaseCtrl:__ctor()
end

function BaseCtrl:__dtor()
end

function BaseCtrl:initialize()
end

function BaseCtrl:log(data)
    __log(data)
end

function BaseCtrl:error(data)
    __error(data)
end

return BaseCtrl

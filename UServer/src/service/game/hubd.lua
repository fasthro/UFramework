-- @Author: fasthro
-- @Date:   2020-11-23 18:30:07
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-23 19:16:24

local skynet = require "skynet"
local cluster = require "skynet.cluster"
require "skynet.manager"

local nodename = ...

local CMD = {}

local subid = 1

function CMD.access(token)
    subid = subid + 1
    token.subid = subid
    return subid
end

skynet.start(function()
    skynet.dispatch("lua", function(_, _, command, ...)
        local f = assert(CMD[command])
        skynet.retpack(f(...))
    end)
    skynet.register("." .. SERVICE_NAME)
end)

-- @Author: fasthro
-- @Date:   2020-11-24 16:55:49
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-24 17:36:48
local skynet = require "skynet"

local function log(str, level, color)
    return function(...)
        local msgs = table.pack(...)
        skynet.error(str, table.unpack(msgs))
    end
end

local logger = {
    debug = log("[debug]", 0, "\x1b[32m"),
    error = log("[error]", 0, "\x1b[31m"),
    info = log("[info]", 0, "\x1b[34m"),
    warning = log("[warning]", 0, "\x1b[33m")
}

_G.logger = logger

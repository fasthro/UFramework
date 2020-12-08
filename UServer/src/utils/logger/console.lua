-- @Author: fasthro
-- @Date:   2020-11-24 16:55:49
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-08 11:45:33
-- 日志系统
local skynet = require "skynet"
local logging = require "logger.logging"

function logging.console()
    return logging.new(function(self, level, message)
        skynet.error(logging.prepareLogMsg(level, message))
        return true
    end)
end

local console = logging.console()
local logger = {}

function logger.debug(...)
    console:debug(...)
end

function logger.info(...)
    console:info(...)
end

function logger.error(...)
    console:error(...)
end

function logger.warning(...)
    console:warn(...)
end

function logger.tostring(value)
    return logging.tostring(value)
end

return logger

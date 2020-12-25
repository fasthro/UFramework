-- @Author: fasthro
-- @Date:   2020-11-24 16:55:49
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-25 15:57:54
-- 日志系统
local skynet = require "skynet"
local logging = require "logger.logging"

local prepare = function(level, message)
    local pattern = "[%level] %message"
    message = string.gsub(message, "%%", "%%%%")
    pattern = string.gsub(pattern, "%%level", "" .. level .. "")
    pattern = string.gsub(pattern, "%%message", message)
    return pattern
end

local console = logging.new(function(self, level, message)
    skynet.error(prepare(level, message))
    return true
end)

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

function logger.buffer_tostring(buffer)
    local res = "["
    local len = #buffer
    for k = 1, len do
        res = res .. buffer:byte(k)
        if k ~= len then
            res = res .. ","
        end
    end
    res = res .. "] size: " .. len
    return res
end

return logger

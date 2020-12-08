--[[
Author: fasthro
Date: 2020-08-28 12:15:46
Description: log
--]]
local logging = require "Logger.Logging"

local prepare = function(pattern, level, message)
    message = string.gsub(message, "%%", "%%%%")
    pattern = string.gsub(pattern, "%%level", level)
    pattern = string.gsub(pattern, "%%message", message)
    return pattern
end

local _logdebug =
    logging.new(
    function(self, level, message)
        local pattern = "<color=#04fd14>[%level]</color> %message"
        UnityEngine.Debug.Log(prepare(pattern, level, message))
        return true
    end
)

local _loginfo =
    logging.new(
    function(self, level, message)
        local pattern = "<color=#49c9c1>[%level]</color> %message"
        UnityEngine.Debug.Log(prepare(pattern, level, message))
        return true
    end
)

local _logerror =
    logging.new(
    function(self, level, message)
        local pattern = "<color=#ff0417>[%level]</color> %message"
        UnityEngine.Debug.LogError(prepare(pattern, level, message))
        return true
    end
)

local _logwarning =
    logging.new(
    function(self, level, message)
        local pattern = "<color=#ebff28>[%level]</color> %message"
        UnityEngine.Debug.LogWarning(prepare(pattern, level, message))
        return true
    end
)

local logger = {}

function logger.debug(...)
    _logdebug:debug(...)
end

function logger.info(...)
    _loginfo:info(...)
end

function logger.error(...)
    _logerror:error(...)
end

function logger.warning(...)
    _logwarning:warn(...)
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

_G.logger = logger

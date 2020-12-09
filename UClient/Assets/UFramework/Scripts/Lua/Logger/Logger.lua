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

-- 日志等级(与C#日志等级同步)
local LEVEL_DEBUG = 0
local LEVEL_INFO = 1
local LEVEL_WARN = 2
local LEVEL_ERROR = 3
local LEVEL_FATAL = 4

-- 当前日志等级
local CUR_LOG_LEVEL = 0

-- 日志等级检查
local checklevel = function(level)
    return level >= CUR_LOG_LEVEL
end

local logger = {}

-- log
function logger.debug(...)
    if checklevel(LEVEL_DEBUG) then
        _logdebug:debug(...)
    end
end

-- info
function logger.info(...)
    if checklevel(LEVEL_INFO) then
        _loginfo:info(...)
    end
end

-- error
function logger.error(...)
    if checklevel(LEVEL_ERROR) then
        _logerror:error(...)
    end
end

-- warning
function logger.warning(...)
    if checklevel(LEVEL_WARN) then
        _logwarning:warn(...)
    end
end

-- set level
function logger.setlevel(level)
    CUR_LOG_LEVEL = level
end

-- tostring
function logger.tostring(value)
    return logging.tostring(value)
end

-- buffer tostring
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

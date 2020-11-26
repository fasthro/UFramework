--[[
Author: fasthro
Date: 2020-11-25 12:21:31
Description: logger
--]]
local logger = {}

function logger.info(arg)
    UnityEngine.Debug.Log(tostring(arg))
end

function logger.debug(arg)
    UnityEngine.Debug.Log(tostring(arg))
end

function logger.warning(arg)
    UnityEngine.Debug.Log(tostring(arg))
end

function logger.error(arg)
    UnityEngine.Debug.Log(tostring(arg))
end

_G.logger = logger

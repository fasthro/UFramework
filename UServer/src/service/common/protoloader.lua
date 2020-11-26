-- @Author: fasthro
-- @Date:   2020-11-24 16:05:53
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-26 16:06:13

local skynet = require "skynet"
local sprotoparser = require "sprotoparser"
local sprotoloader = require "sprotoloader"

local _indexs = {}

local CMD = {}

local function loadfile(name)
    local filename = string.format("src/%s.sproto", name)
    local f = assert(io.open(filename))
    local t = f:read "a"
    f:close()
    return sprotoparser.parse(t)
end

function CMD.load(names)
    for k, name in ipairs(names) do
        _indexs[name] = k
        sprotoloader.save(loadfile(name), k)
    end
end

function CMD.index(name)
    return _indexs[name]
end

skynet.start(function()
    skynet.dispatch("lua", function(_, _, command, ...)
        local f = assert(CMD[command])
        skynet.ret(skynet.pack(f(...)))
    end)
end)


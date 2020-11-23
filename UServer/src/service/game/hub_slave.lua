-- @Author: fasthro
-- @Date:   2020-11-24 12:02:03
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-24 14:24:00
local skynet = require "skynet"
require "skynet.manager"

local hub = ".hubd"

local CMD = {}

function CMD.connect(...)
    skynet_call(hub, "connect", ...)
end

function CMD.logout(c)
    skynet_send(hub, "logout", c)
end

function CMD.kick(data)
    skynet_send(hub, "kick", data)
end

local client = require "client"
local client_handler = client.handler()
function client_handler.handshake(args, fd)
    return skynet_call(hub, "handshake", fd, args)
end

skynet.start(function()

    client.init()
    
    skynet.dispatch("lua", function(session, source, command, ...)
        local f = assert(CMD[command])
        skynet.retpack(f(...))
    end)
end)

-- @Author: fasthro
-- @Date:   2020-11-23 18:42:09
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-24 17:26:15
local skynet = require "skynet"
require "skynet.manager"
local gateserver = require "snax.gateserver"

local hub = "hub_slave"
local connection = {}

skynet.register_protocol {
    name = "client",
    id = skynet.PTYPE_CLIENT
}

local handler = {}

function handler.open(_, c)
end

function handler.connect(fd, addr)
    local c = {
        fd = fd,
        ip = addr
    }
    connection[fd] = c
    skynet_call(hub, "connet", skynet.self(), c)
    gateserver.openclient(fd)
end

function handler.message(fd, msg, sz)
    local c = connection[fd]
    local uid = c.uid
    local source = skynet.self()
    if uid then
        skynet.redirect(c.agent, source, "client", fd, msg, sz)
    else
        skynet.redirect(hub, source, "client", fd, msg, sz)
    end
end

local function close_fd(fd, raw)
    local c = connection[fd]
    if c then
        if not raw then
            skynet_call(hub, "logout", c)
        end
        connection[fd] = nil
        gameserver.closeclient(fd)
    end
end

function handler.disconnect(fd)
    close_fd(fd)
end

function handler.error(fd, msg)
    close_fd(fd)
end

function handler.warning(fd, size)
    
end

local CMD = {}

function CMD.register(_, data)
    local c = assert(connection[data.fd])
    c.uid = data.uid
    c.agent = data.ag
end

function CMD.kick(_, fd, raw)
    close_fd(fd, raw)
end

function handler.command(command, source, ...)
    local f = assert(CMD[command])
    return f(source, ...)
end

gateserver.start(handler)

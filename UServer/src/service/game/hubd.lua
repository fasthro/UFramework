-- @Author: fasthro
-- @Date:   2020-11-23 18:30:07
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-24 15:51:47

local skynet = require "skynet"
local cluster = require "skynet.cluster"
require "skynet.manager"

local nodename = ...

local CMD = {}

local _users = {}
local _fds = {}

local subid = 1

-- 登陆
-- @param token
function CMD.access(token)
    subid = subid + 1
    token.subid = subid
    _users[token.uid] = {token = token, time = os.time()}
    return subid
end

function CMD.connect(gate, c)
    c.gate = gate
    _fds[c.fd] = c
end

function CMD.logout(c)
    if c.uid then
        skynet_send(c.agent, "logout", c)
        cluster.send("login", "logind", "logout", {uid = c.uid})
        _users[c.uid] = nil
    else
        if c.fd then
            _fds[c.fd] = nil
        end
    end
end

function CMD.kick(data)
    local user = _users[data.uid]
    if user then
        skynet_send(user.connection.gate, "kick", user.connection.fd)
    end
end

function CMD.handshake(fd, args)
    local c = _fds[fd]
    
    if not c then
        return 1
    end
    
    local user = _users[args.uid]
    if not user then
        return 2
    end
    
    local token = user.token
    if token.secret ~= args.secret or tostring(token.subid) ~= args.subid then
        return 3
    end
    
    local reconnect = false
    if user.connection then
        reconnect = true
        skynet_send(user.connection.gate, "kick", user.connection.fd, true)
    else
        user.connection = table.clone(c)
        user.agent = skynet_call(".agentpool", "get")
    end
    
    local sip = string.split(user.connection.ip, ':')
    local role = skynet_call(user.agent, "start", {
        fd = user.connection.fd,
        ip = sip[1],
        port = sip[2],
        secret = user.token.secret,
        reconnect = reconnect,
        nodename = nodename
    })
    
    local result = skynet_call(user.connection.gate, "register", {
        fd = fd,
        uid = user.token.uid,
        agent = user.agent
    })
    
    assert(result)
    _fds[fd] = nil
    return SYSTEM_ERROR_CODE.SUCCEED, {role = role}
end

skynet.start(function()
    skynet.dispatch("lua", function(_, _, command, ...)
        local f = assert(CMD[command])
        skynet.retpack(f(...))
    end)
    skynet.register("." .. SERVICE_NAME)
end)

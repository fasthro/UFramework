local loginserver = require "loginserver"
local crypt = require "skynet.crypt"
local skynet = require "skynet"
local cluster = require "skynet.cluster"

local runconfig = require "runconfig"

local nodeconf = runconfig.login.conf
local logic = require "logic.login"

local server = {
    host = nodeconf.host,
    port = nodeconf.port,
    multilogin = false, -- disallow multilogin
    name = nodeconf.name,
    instance = tonumber(nodeconf.instance) or 8
}

function server.auth_handler(token)
    -- the token is base64(user)@base64(server):base64(password)
    local user, server, password = token:match("([^@]+)@([^:]+):(.+)")
    user = crypt.base64decode(user)
    server = crypt.base64decode(server)
    password = crypt.base64decode(password)
    assert(password == "password", "Invalid password")
    return server, user
end

function server.login_handler(serverid, uid, secret)
    local nodeserver = logic.getserver(serverid)
    local hub = ".hubd"
    
    -- only one can login, because disallow multilogin
    local last = logic.getonline(uid)
    if last then
        cluster.send(last.server, hub, "kick", {uid = uid})
        logic.removeonline(uid)
    end
    -- 再次确认用户在线状态
    if logic.getonline(uid) then
        error(string.format("user %s is already online", uid))
    end
    
    local ok, subid = pcall(cluster.call, nodeserver.conf.name, hub, "access", {uid = uid, secret = secret, serverid = serverid})
    if not ok then
        error("login gameserver error.")
    end
    
    logic.addonline(uid, {subid = subid, server = nodeserver.conf.name})
    skynet.error(string.format("<login> addr:%s:%s uid:%s secret:%s subid:%s", nodeserver.conf.host, nodeserver.conf.port, uid, secret, subid))
    return subid
end

local CMD = {}

function CMD.register_gate(server, address)
    server_list[server] = address
end

function CMD.logout(uid, subid)
    local u = user_online[uid]
    if u then
        print(string.format("%s@%s is logout", uid, u.server))
        user_online[uid] = nil
    end
end

function server.command_handler(command, ...)
    local f = assert(CMD[command])
    return f(...)
end

loginserver(server)

local skynet = require "skynet"
local runconfig = require "runconfig"
local playerclass = require "player.player"

skynet.register_protocol {
    name = "client",
    id = skynet.PTYPE_CLIENT,
    unpack = skynet.tostring,
}

local brokecachelen = runconfig.brokecachelen

local gate
local userid, subid
local player
local logout_timer, login_time

local function logout()
    if gate then
        skynet.call(gate, "lua", "logout", userid, subid)
    end
    
    if logout_timer then
        helper.remove_timer(logout_timer)
        logout_timer = nil
    end
    
    -- skynet.exit()
end

local CMD = {}

function CMD.login(source, uid, sid, secret, addr)
    -- you may use secret to make a encrypted data stream
    logger.info(string.format("%s is login", uid))
    gate = source
    userid = uid
    subid = sid
    
    player:login(userid)
    
    if logout_timer then
        helper.remove_timer(logout_timer)
        logout_timer = nil
    end
    logout_timer = helper.add_timer(brokecachelen * 100, logout)
end

function CMD.logout(source)
    -- NOTICE: The logout MAY be reentry
    logger.info(string.format("%s is logout", userid))
    logout()
end

-- 连接中断不意味着玩家下线，在一定时间内没有恢复连接，才进行下线处理
function CMD.afk(source)
    -- the connection is broken, but the user may back
    logger.debug(string.format("AFK"))
    player.connection = 0
    -- 但设定了一定的时限，超过时限还没回来的话即刻当作离线退出处理
    if logout_timer then
        helper.remove_timer(logout_timer)
        logout_timer = nil
    end
    logout_timer = helper.add_timer(brokecachelen * 100, logout)
end

-- 连接修复
function CMD.cbk(source, fd, addr)
    logger.debug(string.format("CBK"))
    player.connection = fd
    if logout_timer then
        helper.remove_timer(logout_timer)
        logout_timer = nil
    end
end

skynet.start(function()
    -- 创建玩家
    player = playerclass:new()
    
    -- If you want to fork a work thread , you MUST do it in CMD.login
    skynet.dispatch("lua", function(session, source, command, ...)
        local f = assert(CMD[command])
        skynet.ret(skynet.pack(f(source, ...)))
    end)
    
    skynet.dispatch("client", function(_, _, msg)
        local pbcloader = skynet.uniqueservice("pbcloader")
        local cmd, session, layer, message = skynet.call(pbcloader, "lua", "decode", msg)
        player:on_message(cmd, message)
        local respmessage = player:on_response()
        if respmessage ~= nil then
            local response = skynet.call(pbcloader, "lua", "encode", cmd, session, layer, respmessage)
            skynet.ret(response)
        else
            skynet.ignoreret()
        end
    end)
end)

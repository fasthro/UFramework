local skynet = require "skynet"

local playerclass = require "player.player"

skynet.register_protocol {
    name = "client",
    id = skynet.PTYPE_CLIENT,
    unpack = skynet.tostring,
}

local gate
local userid, subid
local player

local CMD = {}

function CMD.login(source, uid, sid, secret, addr)
    -- you may use secret to make a encrypted data stream
    logger.info(string.format("%s is login", uid))
    gate = source
    userid = uid
    subid = sid
    
    player:login(userid)
end

local function logout()
    if gate then
        skynet.call(gate, "lua", "logout", userid, subid)
    end
    skynet.exit()
end

function CMD.logout(source)
    -- NOTICE: The logout MAY be reentry
    logger.info(string.format("%s is logout", userid))
    logout()
end

-- 连接中断不意味着玩家下线，在一定时间内没有恢复连接，才进行下线处理
function CMD.afk(source)
    -- the connection is broken, but the user may back
    skynet.error(string.format("AFK"))
end

-- 连接修复
function CMD.cbk(source, fd, addr)
    
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
        player:onreceive(skynet.call(pbcloader, "lua", "decode", msg))
        skynet.ret(msg)
    end)
end)

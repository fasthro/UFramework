-- @Author: fasthro
-- @Date:   2020-11-23 16:56:01
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-24 11:24:56

local runconfig = require "runconfig"

local login = {}
local user_online = {} -- 记录在线玩家

-- 获取服务器
-- @param serverid
function login.getserver(serverid)
    local name = login.getnodename(serverid)
    return runconfig[name]
end

-- 获取服务器节点名称
-- @param serverid
function login.getnodename(serverid)
    return "node" .. serverid
end

-- 添加在线用户
-- @param uid
-- @paramu user
function login.addonline(uid, user)
    user_online[uid] = user
end

-- 移除在线用户
-- @param uid
function login.removeonline(uid)
    user_online[uid] = nil
end

-- 获取在线用户
-- @param uid
function login.getonline(uid)
    return user_online[uid]
end

return login

-- @Author: fasthro
-- @Date:   2020-12-07 16:54:55
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-25 18:09:25
local socket = require "skynet.socket"

local player_session = {}

function player_session:init_session()
    self.connection = 0
end

function player_session:on_message(msgid, msg)
    logger.info(string.format("onreceive userid: %s, msgid: %s, session: %s, msg: %s", self.userid, msgid, session, logger.tostring(msg)))
end

function player_session:on_response()
    return nil
end

function player_session:push(cmd, message, layer)
    if self.connection ~= 0 then
        layer = layer or 1 -- lua 层处理
        local pbcloader = skynet.uniqueservice("pbcloader")
        local buf = skynet.call(pbcloader, "lua", "encode", cmd, -1, layer, message)
        socket.write(self.connection, buf)
    end
end

return player_session

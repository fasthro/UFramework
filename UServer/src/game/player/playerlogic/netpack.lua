-- @Author: fasthro
-- @Date:   2020-12-07 16:54:55
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-08 16:54:56
local player_netpack = {}

function player_netpack:onreceive(msgid, session, msg)
    logger.info(string.format("onreceive userid: %s, msgid: %s, session: %s, msg: %s", self.userid, msgid, session, logger.tostring(msg)))
    local addressbook = {
        name = "Server",
    }
    return addressbook
end

return player_netpack

-- @Author: fasthro
-- @Date:   2020-12-07 16:54:55
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-08 11:51:25
local player_netpack = {}

function player_netpack:send()
	
end

function player_netpack:onreceive(msgid, session, msg)
	logger.info(string.format("onreceive userid: %s, msgid: %s, session: %s, msg: %s", self.userid, msgid, session, logger.tostring(msg)))
end

return player_netpack

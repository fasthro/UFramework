-- @Author: fasthro
-- @Date:   2020-11-26 15:49:24
-- @Last Modified by:   cc
-- @Last Modified time: 2021-03-17 17:43:46
local skynet = require "skynet"
local player_login = {}

function player_login:register(userid, sid)
	
end

function player_login:login(userid)
    self.uid = userid
    self.loginTime = os.time()
end

return player_login

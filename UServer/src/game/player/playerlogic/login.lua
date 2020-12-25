-- @Author: fasthro
-- @Date:   2020-11-26 15:49:24
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-25 16:35:39
local player_login = {}

function player_login:login(userid)
    self.uid = userid
    self.loginTime = os.time()
end

return player_login

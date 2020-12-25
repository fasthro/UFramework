-- @Author: fasthro
-- @Date:   2020-11-26 15:14:29
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-25 17:42:51
local player = class("player")

function player:initialize(...)
    self.uid = 0
    self.login_time = 0

    self:init_session()
end

local logics = {"login", "session"}
for _, v in ipairs(logics) do
	local name = "player.playerlogic." .. v
	local logic = require(name)
	player:include(logic)
end

return player


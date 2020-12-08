-- @Author: fasthro
-- @Date:   2020-11-26 15:14:29
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-08 11:21:50
local player = class("player")

function player:initialize(...)
    self.userid = 0
end

local logicnames = {"login", "netpack"}
for _, v in ipairs(logicnames) do
	local name = "player.playerlogic." .. v
	local logic = require(name)
	player:include(logic)
end

return player


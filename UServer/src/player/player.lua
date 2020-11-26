-- @Author: fasthro
-- @Date:   2020-11-26 15:14:29
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-26 16:22:45
local player = class("player")

function player:initialize(...)
    self.userid = 0
end

local login = require "playerlogic.login"
player:include(login)

return player


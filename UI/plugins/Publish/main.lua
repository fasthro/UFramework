--[[
Author: fasthro
Date: 2020-09-24 15:12:48
Description: 
--]]

require(PluginPath..'/utils')
require(PluginPath..'/writer')
local runner = require(PluginPath..'/runner')

function onPublish(handler)
    local code = runner.new(handler)
    code:execute()
end

function onDestroy()

end
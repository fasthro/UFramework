--[[
Author: fasthro
Date: 2020-09-24 15:12:48
Description: 
--]]

require(PluginPath..'/utils')
local runner = require(PluginPath..'/runner')

function onPublish(handler)
    local code = runner.new(handler)
    code:execute()
end

function onDestroy()

end
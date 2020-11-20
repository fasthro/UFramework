local skynet = require "skynet"
local runconfig = require "runconfig"

skynet.start(function()
    -- 启动控制台
    skynet.uniqueservice("debug_console", runconfig.center.consoleport)
    skynet.exit()
end)

local skynet = require "skynet"
local runconfig = require "runconfig"

skynet.start(function()
    -- 启动控制台
    skynet.uniqueservice("debug_console", runconfig.login.consoleport)
    
    -- 启动登陆服
    skynet.newservice("logind")

    skynet.exit()
end)

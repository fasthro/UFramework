local skynet = require "skynet"
local cluster = require "skynet.cluster"
local runconfig = require "runconfig"
local loginconf = runconfig.login

skynet.start(function()
    -- 启动控制台
    skynet.uniqueservice("debug_console", loginconf.consoleport)
    -- 启动登陆服
    skynet.uniqueservice("logind")

    cluster.open(loginconf.conf.name)
    skynet.exit()
end)

local skynet = require "skynet"
local cluster = require "skynet.cluster"
local runconfig = require "runconfig"
local nodename = skynet.getenv("nodename")
local nodeconf = runconfig[nodename]

skynet.start(function()
    -- 启动控制台
    skynet.uniqueservice("debug_console", nodeconf.consoleport)
    -- 启动节点服务
    skynet.uniqueservice("hubd", nodename)

    cluster.open(nodename)
    skynet.exit()
end)

local skynet = require "skynet"
local cluster = require "skynet.cluster"
local runconfig = require "runconfig"

local harborname = skynet.getenv("harborname")
local nodeconf = runconfig[harborname]

skynet.start(function()
	-- 打开集群
    cluster.open(harborname)

    -- 启动控制台
    skynet.uniqueservice("debug_console", nodeconf.consoleport)
    
    skynet.exit()
end)

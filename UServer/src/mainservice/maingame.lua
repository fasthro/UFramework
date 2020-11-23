local skynet = require "skynet"
local cluster = require "skynet.cluster"
local runconfig = require "runconfig"
local nodename = skynet.getenv("nodename")
local nodeconf = runconfig[nodename]

skynet.start(function()
    -- 启动控制台
    skynet.uniqueservice("debug_console", nodeconf.consoleport)
    
    -- 启动protoloader服务
    local protoloader = skynet.uniqueservice("protoloader")
    skynet.call(protoloader, "lua", "load", {"rpc/c2s", "rpc/s2c"})
    
    -- 启动中心服务
    skynet.uniqueservice("hubd", nodename)
    
    -- 启动hub slave服务
    skynet.newservice("hub_slave")
    
    -- 启动gated服务
    local gateservice = skynet.newservice("gated")
    skynet.call(gateservice, "lua", "open", {
        address = nodeconf.conf.host,
        port = nodeconf.conf.port
    })
    
    cluster.open(nodename)
    skynet.exit()
end)

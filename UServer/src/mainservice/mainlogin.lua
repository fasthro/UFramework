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
    
    -- 启动登陆服
    local logind = skynet.newservice("logind", harborname)
    cluster.register(nodeconf.conf.name, logind)
    
    -- 启动agent池
    skynet.uniqueservice("agentpool", nodeconf.agentpool.name, nodeconf.agentpool.maxnum, nodeconf.agentpool.recyremove, runconfig.brokecachelen)
    for _, conf in pairs(nodeconf.gate_list) do
        local gate = skynet.newservice("gated")
        skynet.call(gate, "lua", "open", conf)
    end
    
    -- proto
    local protoloader = skynet.uniqueservice("protoloader")
    skynet.call(protoloader, "lua", "load", nodeconf.proto_list)

    skynet.exit()
end)

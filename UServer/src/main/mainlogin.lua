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
    
    -- 启动gate
    for _, conf in pairs(nodeconf.gate_list) do
        local gate = skynet.newservice("gated")
        skynet.call(gate, "lua", "open", conf)
    end
    
    -- 启动 pbcloader
    skynet.uniqueservice("pbcloader")
    
    -- 启动 db
    local db = skynet.newservice("mongodb")
    skynet.call(db, "lua", "connect", {host = runconfig.database.host, port = runconfig.database.port})
    
    skynet.exit()
end)

local skynet = require "skynet"
local cluster = require "skynet.cluster"
local runconfig = require "runconfig"

skynet.start(function()
    -- 启动控制台
    skynet.uniqueservice("debug_console", nodeconf.consoleport)
    
    -- 启动 db
    local db = skynet.uniqueservice("mongodb")
    skynet.call(db, "lua", "connect", {host = runconfig.database.host, port = runconfig.database.port})
    
    skynet.exit()
end)

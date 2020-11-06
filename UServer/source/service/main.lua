local skynet = require "skynet"

local max_client = 64

skynet.start(function()
    skynet.error("Server start")
    skynet.newservice("debug_console", 8000)
    
    local loginservice = skynet.newservice("logind")
    local gate = skynet.newservice("gated", loginservice)
    skynet.call(gate, "lua", "open", {
        port = 8888,
        maxclient = max_client,
        servername = "Dev"
    })
    
    skynet.exit()
end)

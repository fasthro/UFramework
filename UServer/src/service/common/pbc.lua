local skynet = require "skynet"
local protobuf = require "protobuf"
local runconfig = require "runconfig"

local CMD = {}

local function register(proto)
    local addr = io.open("./src/proto/" .. proto, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)
end

local function initialize()
    for _, v in ipairs(runconfig.protocols) do
        register(v)
    end
end

function CMD.encode(msgname, msg)
    return protobuf.encode(msgname, msg)
end

function CMD.decode(msgname, data)
    return protobuf.decode(msgname, data)
end

skynet.start(function()
    initialize()
    
    skynet.dispatch("lua", function(_, _, command, ...)
        local f = assert(CMD[command])
        skynet.ret(skynet.pack(f(...)))
    end)
end)

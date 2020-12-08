local skynet = require "skynet"
local protobuf = require "protobuf"
local protocols = require "pbc.pb"
local c2s = require "pbc.c2s"
local s2c = require "pbc.s2c"

local CMD = {}

local function register(proto)
    local addr = io.open("./src/proto/pbc/" .. proto, "rb")
    local buffer = addr:read "*a"
    addr:close()
    protobuf.register(buffer)
end

local function initialize()
    for _, v in ipairs(protocols) do
        register(v)
    end
end

function CMD.encode(msgid, msg)
    assert(s2c[msgid], string.format("pbc s2c %d undefine", msgid))
    return protobuf.encode(s2c[msgid], msg)
end

function CMD.decode(data)
    local dlen = #data - 8
    local msgid, session, msgdata = string.unpack(">I4>I4>c" .. tostring(dlen), data)
    assert(c2s[msgid], string.format("pbc c2s %d undefine", msgid))
    return msgid, session, protobuf.decode(c2s[msgid], msgdata)
end

skynet.start(function()
    initialize()
    
    skynet.dispatch("lua", function(_, _, command, ...)
        local f = assert(CMD[command])
        skynet.ret(skynet.pack(f(...)))
    end)
end)

-- @Author: fasthro
-- @Date:   2020-11-24 13:07:26
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-11-24 15:52:14
local skynet = require "skynet"
local socket = require "skynet.socket"
local socketdriver = require "skynet.socketdriver"
local sprotoloader = require "sprotoloader"
local netpack = require "skynet.netpack"

local client = {}

local _handler = {}
local _host = nil
local _sender = nil
local _fd = nil

function client.init(fd)
    _fd = fd
    
    local protoloader = skynet.uniqueservice "protoloader"
    
    local slot1 = skynet.call(protoloader, "lua", "index", "rpc/c2s")
    _host = sprotoloader.load(slot1):host "package"
    
    local slot2 = skynet.call(protoloader, "lua", "index", "rpc/s2c")
    _sender = _host:attach(sprotoloader.load(slot2))
end

function client.handler()
    return _handler
end

function client.sender()
    return _sender
end

function client.resp_pack(fd, pack, ud, response)
    socketdriver.send(fd, netpack.pack(response(pack, ud)))
end

function client.push_pack(protoname, data, ud)
    socketdriver.send(fd, netpack.pack(_sender(protoname, data, 0, ud)))
end

local function request(fd, name, args, response)
    local f = handler[name]
    if f then
        skynet.fork(function()
            if _fd then
                local ok, code, pack = pcall(f, args)
                if ok then
                    code = code or SYSTEM_ERROR_CODE.SUCCEED
                    pack = pack or {}
                    
                    client.resp_pack(fd, pack, code, response)
                else
                    -- TODO error msg
                end
            else
                local ok, code, pack = pcall(f, args, fd)
                if ok then
                    if pack then
                        client.resp_pack(fd, pack, code, response)
                        if code ~= YSTEM_ERROR_CODE.SUCCEED then
                            skynet_send(skynet.self(), "kick", {uid = args.uid})
                        end
                    else
                        socket.close(fd)
                    end
                end
            end
        end)
    else
        -- TODO 无效的client handler
    end
end

local function dispatch(fd, _, type, ...)
    assert(fd == _fd)
    skynet.ignoreret()
    if type == "REQUEST" then
        local ok, result = pcall(request, fd, ...)
        if not ok then
            skynet.error(result)
        end
    else
        assert(type == "RESPONSE")
        error("client doesn't support request client")
    end
end

skynet.register_protocol{
    name = "client",
    id = skynet.PTYPE_CLIENT,
    unpack = function(msg, sz)
        return _host:dispatch(msg, sz)
    end,
    dispatch = dispatch
}

return client

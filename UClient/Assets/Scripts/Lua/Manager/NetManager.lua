--[[
Author: fasthro
Date: 2020-11-16 12:05:10
Description: 网络管理
--]]
local protobuf = require("3rd.pbc.protobuf")
local pb = require("PB.pb")
local c2s = require("PB.c2s")
local s2c = require("PB.s2c")

local NetManager =
    typesys.def.NetManager {
    __super = typesys.BaseManager
}

function NetManager:initialize()
    self:registerPB()
end

-- 注册PB
function NetManager:registerPB()
    for k, v in ipairs(pb) do
        logger.debug("register pb: " .. v)
        local path = IOPath.PathCombine(App.PBDirectory, v)
        local addr = io.open(path, "rb")
        local buffer = addr:read "*a"
        addr:close()
        protobuf.register(buffer)
    end
end

function NetManager:connect(ip, port)
    self._ext:Connecte(ip, port)
end

function NetManager:redirect(ip, port)
    self._ext:Redirect(ip, port)
end

function NetManager:createPack(protocal, cmd)
    cmd = cmd or -1
    return UFramework.Network.SocketPack.AllocateWriter(protocal, cmd)
end

function NetManager:sendPack(pack)
    self._ext:Send(pack)
end

function NetManager:sendPBC(cmd, data)
    local msg = protobuf.encode(c2s[cmd], data)
    if msg ~= nil then
        local pack = self:createPack(PROTOCAL_TYPE.SIZE_HEADER_BINARY, cmd)
        pack:WriteBuffer(msg .. string.pack(">I4", pack.session))
        self:sendPack(pack)
    end
end

function NetManager:onSocketConnected()
    print("onSocketConnected.")
    EventManager:broadcast(EVENT_NAMES.NET_CONNECTED)
end

function NetManager:onSocketDisconnected()
    print("onSocketDisconnected.")
    EventManager:broadcast(EVENT_NAMES.NET_DISCONNECTED)
end

function NetManager:onSocketException(exception)
    print("onSocketException.[" .. exception .. "]")
    EventManager:broadcast(EVENT_NAMES.NET_EXCEPTION)
end

function NetManager:onSocketReceive(pack)
    EventManager:broadcast(EVENT_NAMES.NET_RECEIVED, pack)
end

return NetManager

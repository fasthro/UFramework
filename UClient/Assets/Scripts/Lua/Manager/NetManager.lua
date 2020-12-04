--[[
Author: fasthro
Date: 2020-11-16 12:05:10
Description: 网络管理
--]]
local protobuf = require("3rd.pbc.protobuf")
local pb = require("PB.pb")

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

-- function NetManager:sendSproto(reqname, arg)
--     local code, tag = self.sproto_c2s:request_encode(reqname, arg)
--     local pack_str = string.pack(">I4", tag) .. string.pack("c" .. #code, code) .. string.pack(">I4", self.session)
-- end

function NetManager:sendPBC(msgname, cmd, data)
    local value = protobuf.encode(msgname, data)
    if value ~= nil then
        local pack = self:createPack(PROTOCAL_TYPE.PBC, cmd)
        pack:WriteBuffer(value)
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

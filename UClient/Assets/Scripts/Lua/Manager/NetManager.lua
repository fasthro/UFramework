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
        local path = IOPath.PathCombine(UApplication.PBDirectory, v)
        local addr = io.open(path, "rb")
        local buffer = addr:read "*a"
        addr:close()
        protobuf.register(buffer)
    end
end

function NetManager:pbcEncode(cmd, session, message)
    assert(c2s[cmd], string.format("pbc c2s %d undefine", cmd))
    local message = protobuf.encode(c2s[cmd], message)
    if message ~= nil then
        return message .. string.pack(">I4", session)
    end
    return nil
end

function NetManager:pbcDecode(cmd, buffer)
    assert(s2c[cmd], string.format("pbc s2c %d undefine", cmd))
    local message = protobuf.decode(s2c[cmd], buffer:sub(1, -6))
    if message ~= nil then
        return message
    end
    return nil
end

function NetManager:connect(ip, port)
    self._ext:Connecte(ip, port)
end

function NetManager:redirect(ip, port)
    self._ext:Redirect(ip, port)
end

function NetManager:createPack(protocal, cmd)
    cmd = cmd or -1
    return UFramework.Core.SocketPack.AllocateWriter(protocal, cmd)
end

function NetManager:sendPack(pack)
    self._ext:Send(pack)
end

function NetManager:sendPBC(cmd, message)
    local pack = self:createPack(PROTOCAL_TYPE.SIZE_HEADER_BINARY, cmd)
    message = self:pbcEncode(cmd, pack.session, message)
    if message ~= nil then
        pack:WriteBuffer(message)
        self:sendPack(pack)
    else
        pack:Recycle()
        logger.error("netmanager send pack error. cmd: " .. cmd)
    end
end

function NetManager:onSocketConnected()
    logger.debug("onSocketConnected.")
    EventManager:broadcast(EVENT_NAMES.NET_CONNECTED)
end

function NetManager:onSocketDisconnected()
    logger.debug("onSocketDisconnected.")
    EventManager:broadcast(EVENT_NAMES.NET_DISCONNECTED)
end

function NetManager:onSocketException(exception)
    logger.debug("onSocketException.[" .. exception .. "]")
    EventManager:broadcast(EVENT_NAMES.NET_EXCEPTION)
end

function NetManager:onSocketReceive(pack)
    logger.debug("onSocketReceive.[" .. pack.cmd .. "]")
    if pack.cmd > 0 then
        local message = self:pbcDecode(pack.cmd, pack.luaRawData)
        if message ~= nil then
            EventManager:broadcast(EVENT_NAMES.NET_RECEIVED, pack.cmd, message)
        else
            logger.error("netmanager receive pack error. cmd: " .. pack.cmd)
        end
        pack:Recycle()
    else
        EventManager:broadcast(EVENT_NAMES.NET_RECEIVED, -1, pack)
    end
end

return NetManager

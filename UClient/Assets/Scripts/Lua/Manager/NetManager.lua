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

    self:createChannel(NETWORK_CHANNEL_TYPE.LOGIN, NETWORK_PROTOCAL_TYPE.TCP)
    self:createChannel(NETWORK_CHANNEL_TYPE.GAME, NETWORK_PROTOCAL_TYPE.TCP)
    self:createChannel(NETWORK_CHANNEL_TYPE.BATTLE, NETWORK_PROTOCAL_TYPE.UDP)
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

function NetManager:createChannel(channeld, protocalType)
    self._ext:CreateChannel(channeld, protocalType)
end

function NetManager:connect(channeld, ip, port)
    self._ext:Connect(channeld, ip, port)
end

function NetManager:redirect(channeld, ip, port)
    self._ext:Redirect(channeld, ip, port)
end

function NetManager:disconnect(channeld)
    self._ext:Disconnect(channeld)
end

function NetManager:createPack(protocal, cmd)
    cmd = cmd or -1
    return UFramework.Network.SocketPack.AllocateWriter(protocal, cmd)
end

function NetManager:_sendPack(channelId, pack)
    self._ext:Send(channelId, pack)
end

function NetManager:sendLoginPack(pack)
    self._ext:Send(NETWORK_CHANNEL_TYPE.LOGIN, pack)
end

function NetManager:sendGamePack(pack)
    self._ext:Send(NETWORK_CHANNEL_TYPE.GAME, pack)
end

function NetManager:sendBattlePack(pack)
    self._ext:Send(NETWORK_CHANNEL_TYPE.BATTLE, pack)
end

function NetManager:_sendPBC(channeld, cmd, message)
    local pack = self:createPack(NETWORK_PACK_TYPE.SIZE_HEADER_BINARY, cmd)
    message = self:pbcEncode(cmd, pack.session, message)
    if message ~= nil then
        pack:WriteBuffer(message)
        self:_sendPack(channeld, pack)
    else
        pack:Recycle()
        logger.error("netmanager send pack error. cmd: " .. cmd)
    end
end

function NetManager:sendGamePBC(cmd, message)
    self:_sendPBC(NETWORK_CHANNEL_TYPE.GAME, cmd, message)
end

function NetManager:sendBattlePBC(cmd, message)
    self:_sendPBC(NETWORK_CHANNEL_TYPE.BATTLE, cmd, message)
end

function NetManager:onSocketConnected(channelId)
    logger.debug("lua socket connected channelId: " .. tostring(channelId))
    EventManager:broadcast(EVENT_NAMES.NET_CONNECTED)
end

function NetManager:onSocketDisconnected(channelId)
    logger.debug("lua socket disconnected channelId: " .. tostring(channelId))
    EventManager:broadcast(EVENT_NAMES.NET_DISCONNECTED)
end

function NetManager:onSocketException(channelId, error)
    logger.debug("lua socket exception channelId: " .. tostring(channelId) .. " error: " .. tostring(error))
    EventManager:broadcast(EVENT_NAMES.NET_EXCEPTION)
end

function NetManager:onSocketReceive(channelId, pack)
    logger.debug("lua socket receive channelId: " .. tostring(channelId) .. " cmd: " .. tostring(pack.cmd))
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

--[[
Author: fasthro
Date: 2020-11-16 12:05:10
Description: 网络管理
--]]
local NetManager =
    typesys.def.NetManager {
    __super = typesys.BaseManager
}

function NetManager:initialize()
end

function NetManager:connect(ip, port)
    self._ext:Connecte(ip, port)
end

function NetManager:setPackOption(option)
    self._ext:SetPackOption(option)
end

function NetManager:sendBytes(value)
    self._ext:Send(value)
end

function NetManager:sendString(value)
    self._ext:Send(value, nil)
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

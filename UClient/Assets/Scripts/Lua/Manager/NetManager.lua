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

function NetManager:sendBytes(value)
    self._ext:Send(value)
end

function NetManager:sendString(value)
    self._ext:Send(value, nil)
end

function NetManager:onConnected()
    print("onConnected.")
    EventManager:broadcast(EVENT_NAMES.NetConnected)
end

function NetManager:onDisconnected()
    print("onDisconnected.")
    EventManager:broadcast(EVENT_NAMES.NetDisconnected)
end

function NetManager:onNetworkError()
    print("onNetworkError.")
end

function NetManager:onReceive(pack)
    EventManager:broadcast(EVENT_NAMES.NetReceived, pack)
end

return NetManager

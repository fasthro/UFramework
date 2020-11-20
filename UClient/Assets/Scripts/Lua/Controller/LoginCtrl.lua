--[[
Author: fasthro
Date: 2020-11-11 17:19:01
Description: 登录控制
--]]
local crypt = require("crypt")

local LoginCtrl =
    typesys.def.LoginCtrl {
    __super = typesys.BaseCtrl
}

function LoginCtrl:__ctor()
    LoginCtrl.__super.__ctor(self)
end

function LoginCtrl:__dtor()
end

function LoginCtrl:initialize()
    PanelManager:showPanel(typesys.LoginPanel)
end

function LoginCtrl:login()
    EventManager:once(EVENT_NAMES.NetConnected, self.onConnected, self)
    EventManager:once(EVENT_NAMES.NetReceived, self.onReceived, self)
    NetManager:connect("192.168.1.171", 8001)
end

function LoginCtrl:onConnected()
end

function LoginCtrl:onReceived(pack)
    local spack = pack:ToStreamPack()
    local code = spack:GetString()
    print("---------------- > " .. code)
    local challenge = crypt.base64decode(code)
    local clientkey = crypt.randomkey()
    local handshake_client_key = crypt.base64encode(crypt.dhexchange(self.clientkey))
    local buffer = handshake_client_key .. "\n"
    NetManager:sendBytes(buffer)
end

return LoginCtrl

--[[
Author: fasthro
Date: 2020-11-11 17:19:01
Description: 登录控制
--]]
--[[
 
Protocol:
 
line (\n) based text protocol
 
1. Server->Client : base64(8bytes random challenge)
2. Client->Server : base64(8bytes handshake client key)
3. Server: Gen a 8bytes handshake server key
4. Server->Client : base64(DH-Exchange(server key))
5. Server/Client secret := DH-Secret(client key/server key)
6. Client->Server : base64(HMAC(challenge, secret))
7. Client->Server : DES(secret, base64(token))
8. Server : call auth_handler(token) -> server, uid (A user defined method)
9. Server : call login_handler(server, uid, secret) ->subid (A user defined method)
10. Server->Client : 200 base64(subid)
 
Error Code:
401 Unauthorized . unauthorized by auth_handler
403 Forbidden . login_handler failed
406 Not Acceptable . already in login (disallow multi login)
 
Success:
200 base64(subid)
]]
local crypt = UFramework.Crypt

local LoginCtrl =
    typesys.def.LoginCtrl {
    __super = typesys.BaseCtrl,
    _username = "",
    _password = "",
    _serverid = "",
    _status = 0, -- 登录状态
    _authStatus = 0, -- 授权状态
    _challenge = typesys.__unmanaged,
    _clientkey = typesys.__unmanaged,
    _secret = typesys.__unmanaged
}

function LoginCtrl:__ctor()
    LoginCtrl.__super.__ctor(self)
end

function LoginCtrl:__dtor()
end

function LoginCtrl:initialize()
    self._status = LOGIN_STATUS.LOGOUT
    PanelManager:showPanel(typesys.LoginPanel)
end

function LoginCtrl:login(username, password, serverid)
    if self._status == LOGIN_STATUS.LOGINED then
        logger.warning("账号已经登录.")
        return
    end

    EventManager:once(EVENT_NAMES.NET_DISCONNECTED, self.onDisconnected, self)
    EventManager:once(EVENT_NAMES.NET_EXCEPTION, self.onDisconnected, self)
    EventManager:add(EVENT_NAMES.NET_RECEIVED, self.onReceived, self)
    self._username = username
    self._password = password
    self._serverid = serverid
    self._authStatus = LOGIN_AUTHOR_STATUS.CHALLENGE
    NetManager:connect("192.168.1.171", 8001)
    NetManager:setProtocalBinary(true)
end

function LoginCtrl:onDisconnected()
    self:removeEvent()
end

function LoginCtrl:onReceived(pack)
    local code = pack:ReadString()

    if self._authStatus == LOGIN_AUTHOR_STATUS.CHALLENGE then
        -- challenge
        self._challenge = crypt.Base64Decode(code)
        self._clientkey = crypt.RandomKey()
        self:sendCode(crypt.Base64Encode(crypt.DHExchange(self._clientkey)))
        self._authStatus = LOGIN_AUTHOR_STATUS.HANDSHAKE_KEY
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.HANDSHAKE_KEY then
        -- handshakeKey
        self._secret = crypt.DHSecret(crypt.Base64Decode(code), self._clientkey)
        self:sendCode(crypt.Base64Encode(crypt.HMAC64(self._challenge, self._secret)))

        -- token
        local token = string.format("%s@%s:%s", crypt.Base64Encode(self._username), crypt.Base64Encode(self._serverid), crypt.Base64Encode(self._password))
        self:sendCode(crypt.Base64Encode(crypt.DesEncode(self._secret, token)))

        self._authStatus = LOGIN_AUTHOR_STATUS.AUTH_RESULT
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.AUTH_RESULT then
        -- auth result
        local result = tonumber(string.sub(code, 1, 3))
        if result == LOGIN_AUTHOR_CODE.SUCCEED then
            AlertCtrl:createLine("账号登录成功")
            self._status = LOGIN_STATUS.LOGINED
            NetManager:setProtocalBinary(false)
            EventManager:broadcast(EVENT_NAMES.LOGIN_SUCCEED)
        else
            AlertCtrl:createLine(string.format("账号登录失败.[%s]", result))
            EventManager:broadcast(EVENT_NAMES.LOGIN_FAILED, result)
        end
        self:removeEvent()
    end
end

function LoginCtrl:sendCode(code)
    local pack = NetManager:createPack(PROTOCAL_TYPE.BINARY)
    pack:WriteBuffer(code .. "\n")
    NetManager:sendPack(pack)
end

function LoginCtrl:removeEvent()
    EventManager:remove(EVENT_NAMES.NET_DISCONNECTED, self.onDisconnected)
    EventManager:remove(EVENT_NAMES.NET_EXCEPTION, self.onDisconnected)
    EventManager:remove(EVENT_NAMES.NET_RECEIVED, self.onReceived)
end

return LoginCtrl

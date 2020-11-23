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
    _status = 0, -- 账号验证状态
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
    PanelManager:showPanel(typesys.LoginPanel)
end

function LoginCtrl:login(username, password, serverid)
    EventManager:once(EVENT_NAMES.NetConnected, self.onConnected, self)
    EventManager:add(EVENT_NAMES.NetReceived, self.onReceived, self)
    self._username = username
    self._password = password
    self._serverid = serverid
    self._status = LOGIN_AUTHOR_STATUS.CHALLENGE
    NetManager:connect("192.168.1.171", 8001)
end

function LoginCtrl:onConnected()
end

function LoginCtrl:onReceived(pack)
    local spack = pack:ToStreamPack()
    local code = spack:GetString()

    if self._status == LOGIN_AUTHOR_STATUS.CHALLENGE then
        -- challenge
        self._challenge = crypt.Base64Decode(code)
        self._clientkey = crypt.RandomKey()
        self:sendCode(crypt.Base64Encode(crypt.DHExchange(self._clientkey)))
        self._status = LOGIN_AUTHOR_STATUS.HANDSHAKE_KEY
    elseif self._status == LOGIN_AUTHOR_STATUS.HANDSHAKE_KEY then
        -- handshakeKey
        self._secret = crypt.DHSecret(crypt.Base64Decode(code), self._clientkey)
        self:sendCode(crypt.Base64Encode(crypt.HMAC64(self._challenge, self._secret)))

        -- token
        local token = string.format("%s@%s:%s", crypt.Base64Encode(self._username), crypt.Base64Encode(self._serverid), crypt.Base64Encode(self._password))
        self:sendCode(crypt.Base64Encode(crypt.DesEncode(self._secret, token)))

        self._status = LOGIN_AUTHOR_STATUS.AUTH_RESULT
    elseif self._status == LOGIN_AUTHOR_STATUS.AUTH_RESULT then
        -- authResult
        local result = tonumber(string.sub(code, 1, 3))

        if result == LOGIN_AUTHOR_CODE.SUCCEED then
            print("login result: 成功")
        elseif result == LOGIN_AUTHOR_CODE.UNAUTHORIZED then
            print("login result: 授权失败")
        elseif result == LOGIN_AUTHOR_CODE.FORBIDDERN then
            print("login result: 无权访问")
        elseif result == LOGIN_AUTHOR_CODE.ALREADY then
            print("login result: 重复登录")
        end
    end
end

function LoginCtrl:sendCode(code)
    NetManager:sendString(code .. "\n")
end

return LoginCtrl

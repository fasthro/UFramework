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
--[[
    登录流程
    1.连接登陆服务器进行握手授权验证
    2.登陆服务器授权登录成功之后连接游戏服务器
    3.游戏服务器连接成功之后要进行一次握手验证
    4.成功之后才可进行协议正常收发
]]
local crypt = UFramework.Crypt

local LoginCtrl =
    typesys.def.LoginCtrl {
    __super = typesys.BaseCtrl,
    _username = "",
    _password = "",
    _serverid = "",
    _subid = typesys.__unmanaged,
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

-- 连接登录服务器
function LoginCtrl:connectLoginServer(ip, port, username, password, serverid)
    EventManager:once(EVENT_NAMES.NET_DISCONNECTED, self.onDisconnected, self)
    EventManager:once(EVENT_NAMES.NET_EXCEPTION, self.onDisconnected, self)
    EventManager:add(EVENT_NAMES.NET_RECEIVED, self.onReceived, self)
    self._username = username
    self._password = password
    self._serverid = serverid
    self._authStatus = LOGIN_AUTHOR_STATUS.CHALLENGE
    NetManager:connect(ip, port)
end

-- 连接游戏服务器
function LoginCtrl:connectGameServer(ip, port)
    if self._status == LOGIN_STATUS.LOGINED then
        self._authStatus = LOGIN_AUTHOR_STATUS.CONNECT_GAME_SERVER

        EventManager:once(EVENT_NAMES.NET_CONNECTED, self.onConnected, self)
        NetManager:redirect(ip, port)
    end
end

function LoginCtrl:onConnected()
    -- logger.info("游戏服连接成功")
    -- PanelManager:hidePanel(typesys.LoginPanel)
    -- PanelManager:showPanel(typesys.MainPanel)

    if self._authStatus == LOGIN_AUTHOR_STATUS.CONNECT_GAME_SERVER then
        self._authStatus = LOGIN_AUTHOR_STATUS.AUTH_GAME_SERVER
        local handshake = string.format("%s@%s#%s:%d", crypt.Base64Encode(self._username), crypt.Base64Encode(self._serverid), crypt.Base64Encode(self._subid), 1)
        local hmac = crypt.HMAC64(crypt.HashKey(handshake), self._secret)

        local pack = NetManager:createPack(PROTOCAL_TYPE.SIZE_BINARY)
        local value = string.format("%s:%s", handshake, crypt.Base64Encode(hmac))
        pack:WriteBuffer(value)
        NetManager:sendPack(pack)

        -- local pack = NetManager:createPack(PROTOCAL_TYPE.BINARY)
        -- local value = string.format("%s:%s", handshake, crypt.Base64Encode(hmac))
        -- pack:WriteBuffer(string.pack(">I2", #value) .. value)
        -- NetManager:sendPack(pack)
    end
end

function LoginCtrl:onDisconnected()
    self:removeEvent()
end

function LoginCtrl:onReceived(pack)
    local code = tostring(pack.luaRawData)
    if self._authStatus == LOGIN_AUTHOR_STATUS.CHALLENGE then
        -- challenge
        self._challenge = crypt.Base64Decode(code)
        self._clientkey = crypt.RandomKey()
        self:sendPack(crypt.Base64Encode(crypt.DHExchange(self._clientkey)) .. "\n")
        self._authStatus = LOGIN_AUTHOR_STATUS.HANDSHAKE_KEY
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.HANDSHAKE_KEY then
        -- handshakeKey
        self._secret = crypt.DHSecret(crypt.Base64Decode(code), self._clientkey)
        self:sendPack(crypt.Base64Encode(crypt.HMAC64(self._challenge, self._secret)) .. "\n")

        -- token
        local token = string.format("%s@%s:%s", crypt.Base64Encode(self._username), crypt.Base64Encode(self._serverid), crypt.Base64Encode(self._password))
        self:sendPack(crypt.Base64Encode(crypt.DesEncode(self._secret, token)) .. "\n")

        self._authStatus = LOGIN_AUTHOR_STATUS.AUTH_RESULT
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.AUTH_RESULT then
        -- auth result
        local result = tonumber(string.sub(code, 1, 3))
        if result == LOGIN_AUTHOR_CODE.SUCCEED then
            logger.info("登录服务器授权验证成功")

            self._status = LOGIN_STATUS.LOGINED
            self._subid = crypt.Base64Decode(string.sub(code, 5))

            EventManager:broadcast(EVENT_NAMES.LOGIN_SUCCEED)
        else
            logger.info("登录服务器授权验证失败 ERROR: " .. result)
            EventManager:broadcast(EVENT_NAMES.LOGIN_FAILED, result)
        end
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.AUTH_GAME_SERVER then
        -- code = self:_unpack_package(code)
        -- auth game server
        local result = tonumber(string.sub(code, 1, 3))
        if result == LOGIN_AUTHOR_CODE.SUCCEED then
            logger.info("游戏服务器授权验证成功，可以正常收发协议")
        else
            logger.info("游戏服务器授权验证失败 ERROR: " .. result)
        end
    end
end

function LoginCtrl:_unpack_package(text)
    local size = #text
    if size < 2 then
        return nil, text
    end
    local s = text:byte(1) * 256 + text:byte(2)
    if size < s + 2 then
        return nil, text
    end
    return text:sub(3, 2 + s), text:sub(3 + s)
end

function LoginCtrl:sendPack(data)
    local pack = NetManager:createPack(PROTOCAL_TYPE.BINARY)
    pack:WriteBuffer(data)
    NetManager:sendPack(pack)
end

function LoginCtrl:removeEvent()
    EventManager:remove(EVENT_NAMES.NET_DISCONNECTED, self.onDisconnected)
    EventManager:remove(EVENT_NAMES.NET_EXCEPTION, self.onDisconnected)
    EventManager:remove(EVENT_NAMES.NET_RECEIVED, self.onReceived)
end

return LoginCtrl

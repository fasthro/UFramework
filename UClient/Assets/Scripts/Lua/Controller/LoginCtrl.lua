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
-- @param ip
-- @param port
-- @param username
-- @param password
-- @param serverid
function LoginCtrl:connectLoginServer(ip, port, username, password, serverid)
    self:removeEvent()

    EventManager:add(EVENT_NAMES.NET_DISCONNECTED, self.onDisconnected, self)
    EventManager:add(EVENT_NAMES.NET_EXCEPTION, self.onException, self)
    EventManager:add(EVENT_NAMES.NET_RECEIVED, self.onReceived, self)
    EventManager:add(EVENT_NAMES.NET_CONNECTED, self.onConnected, self)

    self._username = username
    self._password = password
    self._serverid = serverid
    self._authStatus = LOGIN_AUTHOR_STATUS.LOGIN_SERVER_CHALLENGE

    NetManager:connect(ip, port)
end

-- 连接游戏服务器
-- @param ip
-- @param port
function LoginCtrl:connectGameServer(ip, port)
    if self._status == LOGIN_STATUS.LOGINED then
        self._authStatus = LOGIN_AUTHOR_STATUS.GAME_SERVER_CONNECT
        NetManager:redirect(ip, port)
    end
end

-- 网络连接成功
function LoginCtrl:onConnected()
    -- 登录服务器验证成功之后方可进行游戏授权服验证
    if self._authStatus == LOGIN_AUTHOR_STATUS.GAME_SERVER_CONNECT then
        self._authStatus = LOGIN_AUTHOR_STATUS.GAME_SERVER_AUTH
        local handshake = string.format("%s@%s#%s:%d", crypt.Base64Encode(self._username), crypt.Base64Encode(self._serverid), crypt.Base64Encode(self._subid), 1)
        local hmac = crypt.HMAC64(crypt.HashKey(handshake), self._secret)

        -- 游戏服务器验证方式与登陆服务器消息结构不同，前两个字节为消息长度
        local pack = NetManager:createPack(PROTOCAL_TYPE.SIZE_BINARY)
        pack:WriteBuffer(string.format("%s:%s", handshake, crypt.Base64Encode(hmac)))
        NetManager:sendPack(pack)
    end
end

-- 网络连接断开
function LoginCtrl:onDisconnected()
    self:removeEvent()
end

-- 网络连接异常
function LoginCtrl:onException()
    self:removeEvent()
end

-- 网络接收数据包
function LoginCtrl:onReceived(_, pack)
    local code = tostring(pack.luaRawData)
    if self._authStatus == LOGIN_AUTHOR_STATUS.LOGIN_SERVER_CHALLENGE then
        -- 登录服务器 > challenge
        self._challenge = crypt.Base64Decode(code)
        self._clientkey = crypt.RandomKey()
        self:sendAuthCode(crypt.Base64Encode(crypt.DHExchange(self._clientkey)))
        self._authStatus = LOGIN_AUTHOR_STATUS.LOGIN_SERVER_HANDSHAKE_KEY
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.LOGIN_SERVER_HANDSHAKE_KEY then
        -- 登录服务器 > handshakeKey
        self._secret = crypt.DHSecret(crypt.Base64Decode(code), self._clientkey)
        self:sendAuthCode(crypt.Base64Encode(crypt.HMAC64(self._challenge, self._secret)))

        -- 登录服务器 >  token
        local token = string.format("%s@%s:%s", crypt.Base64Encode(self._username), crypt.Base64Encode(self._serverid), crypt.Base64Encode(self._password))
        self:sendAuthCode(crypt.Base64Encode(crypt.DesEncode(self._secret, token)))

        self._authStatus = LOGIN_AUTHOR_STATUS.LOGIN_SERVER_AUTH_RESULT
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.LOGIN_SERVER_AUTH_RESULT then
        -- 登录服务器 > auth result
        local result = tonumber(string.sub(code, 1, 3))
        if result == LOGIN_AUTHOR_CODE.SUCCEED then
            logger.info("登录服务器授权验证成功")

            self._status = LOGIN_STATUS.LOGINED
            self._subid = crypt.Base64Decode(string.sub(code, 5))

            EventManager:broadcast(EVENT_NAMES.LOGIN_SERVER_AUTH_SUCCEED)
        else
            logger.info("登录服务器授权验证失败 ERROR: " .. result)
            EventManager:broadcast(EVENT_NAMES.LOGIN_SERVER_AUTH_FAILED, result)
        end
    elseif self._authStatus == LOGIN_AUTHOR_STATUS.GAME_SERVER_AUTH then
        -- 游戏服务器 > auth
        local result = tonumber(string.sub(code, 1, 3))
        if result == LOGIN_AUTHOR_CODE.SUCCEED then
            logger.info("游戏服务器授权验证成功，可以正常收发协议")
            EventManager:broadcast(EVENT_NAMES.GAME_SERVER_AUTH_SUCCEED)
        else
            logger.info("游戏服务器授权验证失败 ERROR: " .. result)
            EventManager:broadcast(EVENT_NAMES.GAME_SERVER_AUTH_FAILED)
        end
        self:removeEvent()
    end
end

-- 发送授权码
function LoginCtrl:sendAuthCode(data)
    local pack = NetManager:createPack(PROTOCAL_TYPE.BINARY)
    pack:WriteBuffer(data .. "\n")
    NetManager:sendPack(pack)
end

-- 移除事件监听
function LoginCtrl:removeEvent()
    EventManager:remove(EVENT_NAMES.NET_DISCONNECTED, self.onDisconnected)
    EventManager:remove(EVENT_NAMES.NET_EXCEPTION, self.onException)
    EventManager:remove(EVENT_NAMES.NET_RECEIVED, self.onReceived)
    EventManager:remove(EVENT_NAMES.NET_CONNECTED, self.onConnected)
end

return LoginCtrl

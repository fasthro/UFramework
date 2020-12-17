--[[
Author: fasthro
Date: 2020-11-25 11:25:50
Description: 系统常量
--]]
-- 网络类型
_G.NETWORK_PROTOCAL_TYPE = {
    TCP = UFramework.Network.ProtocalType.Tcp,
    UDP = UFramework.Network.ProtocalType.Udp
}

-- 网络数据包类型
_G.NETWORK_PACK_TYPE = {
    BINARY = UFramework.Network.PackType.Binary,
    SIZE_BINARY = UFramework.Network.PackType.SizeBinary,
    SIZE_HEADER_BINARY = UFramework.Network.PackType.SizeHeaderBinary
}

-- 网络通道类型
_G.NETWORK_CHANNEL_TYPE = {
    LOGIN = 0, -- 登陆服务器
    GAME = 1, -- 游戏服务器
    BATTLE = 2 -- 战斗服务器
}

-- UI面板层
_G.PANEL_LAYER = {
    SCNEN = UFramework.UI.Layer.SCNEN,
    PANEL = UFramework.UI.Layer.PANEL,
    MESSAGE_BOX = UFramework.UI.Layer.MESSAGE_BOX,
    GUIDE = UFramework.UI.Layer.GUIDE,
    NOTIFICATION = UFramework.UI.Layer.NOTIFICATION,
    NETWORK = UFramework.UI.Layer.NETWORK,
    LOADER = UFramework.UI.Layer.LOADER,
    TOP = UFramework.UI.Layer.TOP
}

-- 登录授权状态
_G.LOGIN_AUTHOR_STATUS = {
    LOGIN_SERVER_CHALLENGE = 0, -- 登陆服务器
    LOGIN_SERVER_HANDSHAKE_KEY = 1, -- 登陆服务器
    LOGIN_SERVER_AUTH_RESULT = 2, -- 登陆服务器
    GAME_SERVER_CONNECT = 3, -- 游戏服务器
    GAME_SERVER_AUTH = 4 -- 游戏服务器
}

-- 登录授权结果
_G.LOGIN_AUTHOR_CODE = {
    SUCCEED = 200, -- 验证成功
    UNAUTHORIZED = 401, -- 授权失败(auth_handle token 无效)
    FORBIDDERN = 403, -- 登录失败(login_handler 执行失败)
    ALREADY = 406 -- 重复登录
}

-- 登录状态
_G.LOGIN_STATUS = {
    LOGINED = 1, -- 已经登陆
    LOGOUT = 2 -- 离线
}

-- 提示 MessageBox 类型
_G.ALERT_MESSAGEBOX_TYPE = {
    NORMAL = 1
}

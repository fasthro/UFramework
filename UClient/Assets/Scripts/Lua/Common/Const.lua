--[[
Author: fasthro
Date: 2020-11-25 11:25:50
Description: 系统常量
--]]
-- 网络数据包解析选项
_G.PROTOCAL_TYPE = {
    BINARY = UFramework.Network.ProtocalType.Binary,
    LINEAR_BINARY = UFramework.Network.ProtocalType.LinearBinary,
    PBC = UFramework.Network.ProtocalType.PBC,
    PROTOBUF = UFramework.Network.ProtocalType.Protobuf,
    SPROTO = UFramework.Network.ProtocalType.Sproto
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
    CHALLENGE = 0,
    HANDSHAKE_KEY = 1,
    AUTH_RESULT = 2
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

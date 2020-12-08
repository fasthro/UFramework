--[[
Author: fasthro
Date: 2020-11-20 17:23:30
Description: 事件名称定义
--]]
_G.EVENT_NAMES = {
    ------------>> network event << ------------
    NET_CONNECTED = "NetConnected", -- 网络连接成功
    NET_DISCONNECTED = "NetDisconnected", -- 网络已断开
    NET_RECEIVED = "NetReceived", -- 接受网络协议数据
    NET_EXCEPTION = "NetException", -- 网络异常
    ------------>> login event << ------------
    LOGIN_SERVER_AUTH_SUCCEED = "LoginServerAuthSucceed", -- 登录服务器授权成功
    LOGIN_SERVER_AUTH_FAILED = "LoginServerAuthFailed", -- 登录服务器授权失败
    GAME_SERVER_AUTH_SUCCEED = "GameServerAuthSucceed", -- 游戏服授权成功
    GAME_SERVER_AUTH_FAILED = "GameServerAuthFailed" -- 游戏服授权失败
}

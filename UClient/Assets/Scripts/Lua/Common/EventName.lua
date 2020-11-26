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
    LOGIN_SUCCEED = "LoginSucceed", -- 登录成功
    LOGIN_FAILED = "LoginFailed" -- 登录失败
}

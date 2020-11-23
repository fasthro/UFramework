--[[
Author: fasthro
Date: 2020-11-24 11:07:13
Description: 登录常量
--]]
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

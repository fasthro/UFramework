-- @Author: fasthro
-- @Date:   2020-12-25 17:17:20
-- @Last Modified by:   fasthro
-- @Last Modified time: 2020-12-25 17:26:19
local skynet = require "skynet"

local helper = {}

-- 添加定时器
-- @param ti 时间
-- @param f 回调
-- @return timer
function helper.add_timer(ti, f)
    local flag = true
    local function t()
        if not flag then
            return
        end
        f()
    end
    skynet.timeout(ti, t)
    return function() flag = false end
end

-- 移除定时器
-- @param timer 定时器
function helper.remove_timer(timer)
    assert(type(timer) == "function")
    timer()
end

return helper


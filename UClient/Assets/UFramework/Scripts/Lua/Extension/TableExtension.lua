--[[
Author: fasthro
Date: 2020-11-11 15:56:15
Description: table 扩展
--]]
function table.findKey(t, k)
    if t == nil then
        return nil
    end
    for key, value in pairs(t) do
        if k == key then
            return value
        end
    end
    return nil
end

function table.removeKey(t, k)
    if t == nil then
        return nil
    end
    local v = nil
    local tb = {}
    for key, value in pairs(t) do
        if k ~= key then
            tb[tostring(key)] = value
        else
            v = value
        end
    end
    return tb, v
end

function table.reverse(t)
    local tb = {}
    for i = 1, #t do
        local key = #t + 1 - i
        tb[i] = t[key]
    end
    return tb
end

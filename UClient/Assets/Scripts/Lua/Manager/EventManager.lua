--[[
Author: fasthro
Date: 2020-11-20 15:25:09
Description: event manager
--]]
------------------------------------------------------------
-- 监听器包装类
local EventListenerWrap =
    typesys.def.EventListenerWrap {
    _listener = typesys.__unmanaged,
    _owner = typesys.__unmanaged
}

function EventListenerWrap:initialize(listener, owner)
    self._listener = listener
    self._owner = owner
end

function EventListenerWrap:broadcast(...)
    if self._owner ~= nil then
        self._listener(self._owner, ...)
    else
        self._listener(...)
    end
end

function EventListenerWrap:equal(listener)
    return self._listener == listener
end

------------------------------------------------------------
-- 事件包装类
local EventWrap =
    typesys.def.EventWrap {
    _isonce = false,
    _eventname = "",
    _listeners = typesys.array
}

function EventWrap:initialize(eventname, isonce)
    self._listeners = typesys.new(typesys.array, typesys.EventListenerWrap)
    self._isonce = isonce
    self._eventname = eventname
end

function EventWrap:add(listener, owner)
    local wrap = __newdt(typesys.EventListenerWrap)
    wrap:initialize(listener, owner)
    self._listeners:pushBack(wrap)
end

function EventWrap:has(listener)
    for k = 1, self._listeners:size() do
        local v = self._listeners:get(k)
        if v.equal ~= nil and v:equal(listener) then
            return true
        end
    end
    return false
end

function EventWrap:remove(listener)
    for k = 1, self._listeners:size() do
        local v = self._listeners:get(k)
        if v:equal(listener) then
            self._listeners:set(k, nil)
            break
        end
    end
end

function EventWrap:broadcast(...)
    for k = 1, self._listeners:size() do
        self._listeners:get(k):broadcast(...)
    end
end

function EventWrap:equal(eventname)
    return self._eventname == eventname
end

function EventWrap:isonce()
    return self._isonce
end

------------------------------------------------------------
-- 事件管理器
local EventManager =
    typesys.def.EventManager {
    __super = typesys.BaseManager,
    _listeners = typesys.map
}

function EventManager:initialize()
    self._listeners = typesys.new(typesys.map, type(""), typesys.EventWrap)
end

-- 添加事件
-- @param eventname 事件名称
-- @param listener 监听器
-- @param owner 对象
function EventManager:add(eventname, listener, owner)
    self:_create(eventname, listener, owner, false)
end

-- 添加顺时事件（执行一次自动移除）
-- @param eventname 事件名称
-- @param listener 监听器
-- @param owner 对象
function EventManager:once(eventname, listener, owner)
    self:_create(eventname, listener, owner, true)
end

-- 移除事件
-- @param eventname 事件名称
-- @param listener 监听器
function EventManager:remove(eventname, listener)
    if eventname == "" or eventname == nil or #eventname == 0 then
        logger.error("event manager remove eventname is empty.")
        return
    end
    if listener == nil then
        self._listeners:set(eventname, nil)
    else
        local warp = self._listeners:get(eventname)
        if warp ~= nil then
            warp:remove(listener)
        end
    end
end

-- 移除全部事件
function EventManager:removeAll()
    for k, v in pairs(self._listeners) do
        self:remove(k)
    end
    self._listeners:clear()
end

-- 是否存在事件
-- @param eventname 事件名称
-- @param listener 监听器
function EventManager:has(eventname, listener)
    if eventname == "" or eventname == nil or #eventname == 0 or listener == nil then
        return false
    end
    local wrap = self._listeners:get(eventname)
    if wrap ~= nil then
        return wrap:has(listener), wrap
    end
    return false, wrap
end

-- 广播事件
-- @param eventname 事件名称
-- @param ... 参数
function EventManager:broadcast(eventname, ...)
    local warp = self._listeners:get(eventname)
    if warp ~= nil then
        warp:broadcast(...)

        if warp:isonce() then
            self:remove(eventname)
        end
    end
end

function EventManager:_create(eventname, listener, owner, isonce)
    local existed, wrap = self:has(eventname, listener)
    if not existed then
        if wrap == nil then
            wrap = __newdt(typesys.EventWrap)
            wrap:initialize(eventname, isonce)
            self._listeners:set(eventname, wrap)
        end
        wrap:add(listener, owner)
    else
        if eventname == nil or eventname == "" or #eventname == 0 then
            __error(string.format("eventname is empty.", eventname))
        else
            __error(string.format("eventname: %s already existed.", eventname))
        end
    end
end

return EventManager

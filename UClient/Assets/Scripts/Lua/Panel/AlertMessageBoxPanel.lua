--[[
Author: fasthro
Date: 2020-11-26 17:16:31
Description: MessageBox
--]]
require("Module.Alert.AlertMessageBoxModule")
require("Module.Alert.AlertMessageBoxModule1")

local panel =
    typesys.def.AlertMessageBoxPanel {
    __super = typesys.BasePanel,
    _id = 0,
    _tobjs = typesys.map,
    _actives = typesys.__unmanaged, -- 激活列表
    _recycles = typesys.__unmanaged -- 回收列表
}

function panel:__ctor()
    self._name = "AlertMessageBox"
    self._package = "Alert"
    self._layer = PANEL_LAYER.NOTIFICATION
    self._dependences = nil
    self._immortal = true

    panel.__super.__ctor(self)

    self._actives = {}
    self._recycles = {}
    self._tobjs = typesys.new(typesys.map, type(0), typesys.map)
    for k, t in pairs(ALERT_MESSAGEBOX_TYPE) do
        self._tobjs:set(t, typesys.new(typesys.map, type(0), typesys["AlertMessageBoxModule" .. tostring(t)]))
        self._actives[t] = {}
        self._recycles[t] = {}
    end
end

function panel:onAwake()
end

function panel:onDispose()
end

function panel:onShow()
    panel.__super.onShow(self)
end

function panel:onHide()
end

function panel:onNetReceived(pack)
end

-- 创建提示
-- @param description 使用参数说明
function panel:create(desc)
    assert(type(desc) == "table", "message box argument must table type.")
    assert(desc.type ~= nil, "messagebox argument error.")

    local _type = desc.type

    self:_checkRecycle(_type)

    local mb
    if not self:_hasRecycle(_type) then
        self._id = self._id + 1
        mb = __newdt(typesys["AlertMessageBoxModule" .. tostring(_type)], self._id, self:_createView(_type))
        self:_setObj(_type, mb)
    else
        mb = self:_getObj(_type, self:_removeRecycle(_type, 1))
    end
    mb:show(desc)
    self:_setActive(_type, mb:getId())
end

function panel:_createView(_type)
    local com = self:_createObject("MessageBox-" .. _type).asCom
    self.view:AddChild(com)
    com:MakeFullScreen()
    com.x = 0
    com.y = 0
    return com
end

function panel:_checkRecycle(_type)
    local size = self:_getActiveSize(_type)
    local index = 1
    for k = 1, size do
        local mb = self:_getObj(_type, self:_getActive(_type, index))
        if mb:canRecycle() then
            self:_removeActive(_type, index)
            self:_setRecycle(_type, mb:getId())
        else
            index = index + 1
        end
    end
end

function panel:_getObj(_type, id)
    local objs = self._tobjs:get(_type)
    return objs:get(id)
end

function panel:_setObj(_type, obj)
    local objs = self._tobjs:get(_type)
    objs:set(obj:getId(), obj)
end

function panel:_getActive(_type, index)
    return self._actives[_type][index]
end

function panel:_setActive(_type, id)
    table.insert(self._actives[_type], id)
end

function panel:_removeActive(_type, index)
    table.remove(self._actives[_type], index)
end

function panel:_getActiveSize(_type)
    return #self._actives[_type]
end

function panel:_removeRecycle(_type, index)
    local id = self._recycles[_type][index]
    table.remove(self._recycles[_type], index)
    return id
end

function panel:_setRecycle(_type, id)
    table.insert(self._recycles[_type], id)
end

function panel:_hasRecycle(_type)
    return #self._recycles[_type] > 0
end

return panel

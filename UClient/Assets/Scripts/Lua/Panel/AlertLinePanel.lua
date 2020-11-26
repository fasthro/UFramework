--[[
Author: fasthro
Date: 2020-11-26 17:12:19
Description: 单行提示
--]]
require("Module.Alert.AlertLineModule")

local panel =
    typesys.def.AlertLinePanel {
    __super = typesys.BasePanel,
    _id = 0,
    _objs = typesys.map,
    _actives = typesys.__unmanaged, -- 激活列表
    _recycles = typesys.__unmanaged -- 回收列表
}

function panel:__ctor()
    self._name = "AlertLine"
    self._package = "Alert"
    self._layer = PANEL_LAYER.NOTIFICATION
    self._dependences = nil
    self._immortal = true

    panel.__super.__ctor(self)

    self._objs = typesys.new(typesys.map, type(0), typesys.AlertLineModule)
    self._actives = {}
    self._recycles = {}
end

function panel:_onAwake()
end

function panel:_onDispose()
end

function panel:onShow()
    panel.__super.onShow(self)
end

function panel:onHide()
end

function panel:onEventReceive(id)
end

function panel:onNetworReceive()
end

function panel:create(content)
    self:_checkRecycle()

    local line
    if #self._recycles <= 0 then
        self._id = self._id + 1
        line = __newdt(typesys.AlertLineModule, self._id, self:_createItem())
        self._objs:set(self._id, line)
    else
        line = self._objs:get(self._recycles[1])
        table.remove(self._recycles, 1)
    end
    line:born(content)
    table.insert(self._actives, line:getId())
end

function panel:_createItem()
    local item = self:_createObject("LineItem").asCom
    self.view:AddChild(item)
    item.pivotX = 0.5
    item.pivotY = 0.5
    item.x = self.view._root.x
    item.y = self.view._root.y
    item.touchable = false

    return item
end

function panel:_checkRecycle()
    local size = #self._actives
    local index = 1
    for k = 1, size do
        local line = self._objs:get(self._actives[index])
        if line:canRecycle() then
            table.remove(self._actives, index)
            table.insert(self._recycles, line:getId())
        else
            index = index + 1
        end
    end
end

return panel

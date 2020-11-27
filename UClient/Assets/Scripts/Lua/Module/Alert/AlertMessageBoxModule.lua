--[[
Author: fasthro
Date: 2020-11-26 17:51:43
Description: MessageBox 父类
--]]
local AlertMessageBoxModule =
    typesys.def.AlertMessageBoxModule {
    _view = typesys.__unmanaged,
    _id = 0,
    _desc = typesys.__unmanaged,
    _recycle = false
}

function AlertMessageBoxModule:__ctor(id, view)
    self._id = id
    self._view = view
end

function AlertMessageBoxModule:getId()
    return self._id
end

function AlertMessageBoxModule:show(desc)
    self._view.visible = true
    self._desc = desc
    self._recycle = false
    self:_onShow()
end

function AlertMessageBoxModule:hide()
    self._view.visible = false
    self._recycle = true
    self:_onHide()
end

function AlertMessageBoxModule:canRecycle()
    return self._recycle
end

function AlertMessageBoxModule:_onShow()
    -- 子类实现
end

function AlertMessageBoxModule:_onHide()
    -- 子类实现
end

return AlertMessageBoxModule

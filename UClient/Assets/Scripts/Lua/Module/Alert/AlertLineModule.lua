--[[
Author: fasthro
Date: 2020-11-26 17:51:43
Description: 
--]]
local AlertLineModule =
    typesys.def.AlertLineModule {
    _view = typesys.__unmanaged,
    _id = 0,
    _recycle = false
}

function AlertLineModule:__ctor(id, view)
    self._id = id
    self._view = view
end

function AlertLineModule:getId()
    return self._id
end

function AlertLineModule:show(content, delay)
    self._view.visible = true
    self._recycle = false
    self._view._content.text = content
    local ani = self._view._born
    ani:Play(
        1,
        delay,
        function()
            self:wait()
        end
    )
end

function AlertLineModule:wait()
    local ani = self._view._die
    ani:Play(
        1,
        1,
        function()
            self:hide()
        end
    )
end

function AlertLineModule:hide()
    self._view.visible = false
    self._recycle = true
end

function AlertLineModule:canRecycle()
    return self._recycle
end

return AlertLineModule

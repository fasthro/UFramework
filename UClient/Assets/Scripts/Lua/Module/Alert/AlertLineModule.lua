--[[
Author: fasthro
Date: 2020-11-26 17:51:43
Description: 
--]]
local AlertLineModule =
    typesys.def.AlertLineModule {
    _item = typesys.__unmanaged,
    _id = 0,
    _recycle = false
}

function AlertLineModule:__ctor(id, item)
    self._id = id
    self._item = item
end

function AlertLineModule:getId()
    return self._id
end

function AlertLineModule:born(content)
    self._recycle = false
    self._item._content.text = content
    local ani = self._item._born
    ani:Play(
        function()
            self:waitDie()
        end
    )
end

function AlertLineModule:waitDie()
    local ani = self._item._die
    ani:Play(
        1,
        1,
        function()
            self:die()
        end
    )
end

function AlertLineModule:die()
    self._recycle = true
end

function AlertLineModule:canRecycle()
    return self._recycle
end

return AlertLineModule

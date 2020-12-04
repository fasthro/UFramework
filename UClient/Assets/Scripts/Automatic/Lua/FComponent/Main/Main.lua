-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._sendpbc = self:GetChildAt(0).asButton
end

function component:setClick_sendpbc(callback, target)
	self._sendpbc.onClick:Set(callback, target)
end

fgui.register_extension("ui://slaudw4wz4a20", component)

return component

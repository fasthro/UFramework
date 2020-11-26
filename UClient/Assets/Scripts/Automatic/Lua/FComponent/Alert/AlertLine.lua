-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._root = self:GetChildAt(0).asCom
end

fgui.register_extension("ui://i894r9q610ewg0", component)

return component

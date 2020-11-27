-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._background = self:GetChildAt(0).asCom
	self._center = self:GetChildAt(1).asCom
	self._show = self:GetTransitionAt(0)
end

fgui.register_extension("ui://i894r9q610ewg4", component)

return component

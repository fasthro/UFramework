-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._center = self:GetChildAt(1).asCom
	self._show = self:GetTransitionAt(0)
	self._hide = self:GetTransitionAt(1)
end

fgui.register_extension("ui://i894r9q6t33yh0", component)

return component

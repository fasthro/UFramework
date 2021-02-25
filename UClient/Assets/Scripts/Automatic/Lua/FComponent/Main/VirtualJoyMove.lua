-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._bg = self:GetChildAt(0).asImage
	self._touch = self:GetChildAt(1).asImage
	self._touchBegin = self:GetTransitionAt(0)
	self._touchEnd = self:GetTransitionAt(1)
end

fgui.register_extension("ui://slaudw4whftt4", component)

return component

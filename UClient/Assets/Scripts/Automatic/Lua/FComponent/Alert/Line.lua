-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._content = self:GetChildAt(0).asRichTextField
	self._born = self:GetTransitionAt(0)
	self._die = self:GetTransitionAt(1)
end

function component:setText_content(text)
	self._content.text = text
end

fgui.register_extension("ui://i894r9q610ewg1", component)

return component

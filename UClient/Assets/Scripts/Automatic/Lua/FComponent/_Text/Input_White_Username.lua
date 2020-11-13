-- uframework automatically generated
local component = fgui.extension_class(GButton)

function component:ctor()
	self._input = self:GetChildAt(3).asTextInput
end

function component:setText_input(text)
	self._input.text = text
end

fgui.register_extension("ui://5w14mmk5unrx1", component)

return component

-- uframework automatically generated
local component = fgui.extension_class(GButton)

function component:ctor()
	self._state = self:GetControllerAt(0)
	self._input = self:GetChildAt(4).asTextInput
end

function component:setIndex_state(index)
	self._state:SetSelectedIndex(index)
end

function component:setText_input(text)
	self._input.text = text
end

fgui.register_extension("ui://5w14mmk5t33yb", component)

return component

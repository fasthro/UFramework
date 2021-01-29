-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._btn = self:GetControllerAt(0)
	self._title = self:GetChildAt(4).asTextField
	self._content = self:GetChildAt(5).asRichTextField
	self._no = self:GetChildAt(6).asButton
	self._yes = self:GetChildAt(7).asButton
end

function component:setIndex_btn(index)
	self._btn:SetSelectedIndex(index)
end

function component:setText_title(text)
	self._title.text = text
end

function component:setText_content(text)
	self._content.text = text
end

function component:setClick_no(callback, target)
	self._no.onClick:Set(callback, target)
end

function component:setClick_yes(callback, target)
	self._yes.onClick:Set(callback, target)
end

fgui.register_extension("ui://i894r9q6t33yh3", component)

return component

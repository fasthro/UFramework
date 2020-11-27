-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._btn = self:GetControllerAt(0)
	self._title = self:GetChildAt(3).asTextField
	self._content = self:GetChildAt(4).asRichTextField
	self._btn1 = self:GetChildAt(5).asButton
	self._btn2 = self:GetChildAt(6).asButton
	self._btn3 = self:GetChildAt(7).asButton
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

function component:setClick_btn1(callback, target)
	self._btn1.onClick:Set(callback, target)
end

function component:setClick_btn2(callback, target)
	self._btn2.onClick:Set(callback, target)
end

function component:setClick_btn3(callback, target)
	self._btn3.onClick:Set(callback, target)
end

fgui.register_extension("ui://i894r9q610ewg7", component)

return component

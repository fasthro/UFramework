-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._c1 = self:GetControllerAt(0)
	self._n0 = self:GetChildAt(0).asCom
	self._n1 = self:GetChildAt(1).asImage
	self._n3 = self:GetChildAt(2).asButton
	self._n4 = self:GetChildAt(3).asProgress
	self._n5 = self:GetChildAt(4).asTextField
	self._n6 = self:GetChildAt(5).asRichTextField
	self._n7 = self:GetChildAt(6).asTextInput
	self._n8 = self:GetChildAt(7).asGraph
	self._n9 = self:GetChildAt(8).asList
	self._n10 = self:GetChildAt(9).asLoader
	self._n11 = self:GetChildAt(10).asLoader3D
	self._n13 = self:GetChildAt(12).asMovieClip
	self._n14 = self:GetChildAt(13).asSlider
	self._n15 = self:GetChildAt(14).asLabel
	self._n17 = self:GetChildAt(15).asGroup
	self._t0 = self:GetTransitionAt(0)
end

function component:setIndex_c1(index)
	self._c1:SetSelectedIndex(index)
end

function component:setClick_n3(callback, target)
	self._n3.onClick:Set(callback, target)
end

function component:setValue_n4(value, max)
	self._n4.value = value
	self._n4.max = max
end

function component:setText_n5(text)
	self._n5.text = text
end

function component:setText_n6(text)
	self._n6.text = text
end

function component:setText_n7(text)
	self._n7.text = text
end

function component:initVirtualList_n9(renderCallback, clickCallback, target)
	self._n9:SetVirtual()
	self._n9.itemRenderer = renderCallback
	self._n9.onClickItem:Set(clickCallback, target)
	self._n9.numItems = 0
end

function component:setVirtualList_n9(num)
	self._n9.numItems = num
end

function component:setUrl_n10(url)
	self._n10.url = url
end

fui.register_extension("ui://9fwlj8d0gmfc0", component)

return component

-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._gsSendPBC = self:GetChildAt(0).asButton
	self._bsConnect = self:GetChildAt(1).asButton
	self._bsSendPBC = self:GetChildAt(2).asButton
end

function component:setClick_gsSendPBC(callback, target)
	self._gsSendPBC.onClick:Set(callback, target)
end

function component:setClick_bsConnect(callback, target)
	self._bsConnect.onClick:Set(callback, target)
end

function component:setClick_bsSendPBC(callback, target)
	self._bsSendPBC.onClick:Set(callback, target)
end

fgui.register_extension("ui://slaudw4wz4a20", component)

return component

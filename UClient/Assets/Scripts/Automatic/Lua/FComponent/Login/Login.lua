-- uframework automatically generated
local component = fgui.extension_class(GComponent)

function component:ctor()
	self._state = self:GetControllerAt(0)
	self._username = self:GetChildAt(1).asButton
	self._password = self:GetChildAt(2).asButton
	self._login = self:GetChildAt(3).asButton
	self._touch = self:GetChildAt(5).asCom
end

function component:setIndex_state(index)
	self._state:SetSelectedIndex(index)
end

function component:setClick_username(callback, target)
	self._username.onClick:Set(callback, target)
end

function component:setClick_password(callback, target)
	self._password.onClick:Set(callback, target)
end

function component:setClick_login(callback, target)
	self._login.onClick:Set(callback, target)
end

fgui.register_extension("ui://u1w4k91lunrx0", component)

return component
